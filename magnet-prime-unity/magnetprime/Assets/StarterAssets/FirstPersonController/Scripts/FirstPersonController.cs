using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 6.0f;
        [Tooltip("Rotation speed of the character")]
        public float RotationSpeed = 1.0f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        [Tooltip("How fast the player's gun fires")]
        public float FireRate = 0.1f;
        public float grabDistance = 5f;
        public float polarizeDistance = 5f;
        public float polarizeStrength = 10f;
        public int polarity = 1;
        float shootCD = 1;
        bool shootPressed = false;
        GameObject held;
        public Transform grabPoint;
        public LineRenderer lineRenderer;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.1f;
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.5f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 90.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -90.0f;

        // cinemachine
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        public delegate void ShootDelegate(GameObject objectHit);
        public event ShootDelegate InvokeShoot;

        public delegate void PolarizeDelegate(GameObject self, GameObject target);
        public event PolarizeDelegate InvokePolarize;

        public bool lastShot = false;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            shootCD = FireRate;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            Shoot();
        }

        private void FixedUpdate()
        {
            Grab();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }

        private void CameraRotation()
        {
            // if there is an input
            if (_input.look.sqrMagnitude >= _threshold)
            {
                _cinemachineTargetPitch += _input.look.y * RotationSpeed * Time.deltaTime;
                _rotationVelocity = _input.look.x * RotationSpeed * Time.deltaTime;

                // clamp our pitch rotation
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Update Cinemachine camera target pitch
                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

                // rotate the player left and right
                transform.Rotate(Vector3.up * _rotationVelocity);
            }
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                // move
                inputDirection = transform.right * _input.move.x + transform.forward * _input.move.y;
            }

            // move the player
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

        public void Shoot()
        {
            shootCD = Mathf.Clamp(shootCD - Time.deltaTime, 0, FireRate);

            if (shootPressed && shootCD <= 0)
            {
                shootCD = FireRate;
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
                {
                    if (lastShot)
                    {
                        //Debug.Log("Positive Shoot");

                        if (InvokeShoot != null)
                            InvokeShoot(hit.transform.gameObject);
                    }
                    else
                    {
                        //Debug.Log("Negative Shoot");

                        if (InvokeShoot != null)
                            InvokeShoot(hit.transform.gameObject);
                    }
                }
                else
                {
                    //Debug.Log("Did not Hit");

                    if (InvokeShoot != null)
                        InvokeShoot(null);
                }
            }
            else
            {

            }
        }

        public void OnPositiveShoot(InputValue value)
        {
            lastShot = true;
            shootPressed = value.isPressed;
        }

        public void OnNegativeShoot(InputValue value)
        {
            lastShot = false;
            shootPressed = value.isPressed;
        }

        public void OnGrab(InputValue value)
        {
            if (held == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, grabDistance))
                {
                    Magnetism_Movable grabbed = hit.collider.GetComponent<Magnetism_Movable>();
                    Debug.Log(hit.collider.name);
                    if (grabbed != null && grabbed.grabbable == true)
                    {
                        grabbed.SetDragToPlayer(false);
                        grabbed.grabbable = false;
                        StartCoroutine(grabbed.LerpScale(true));
                        held = grabbed.gameObject;
                        held.GetComponent<Rigidbody>().useGravity = false;
                        held.GetComponent<Rigidbody>().mass = 1000;

                        Collider c = held.GetComponent<Collider>();
                        if (c.GetType() == typeof(SphereCollider))
                            held.GetComponent<SphereCollider>().radius = 1;
                        else if (c.GetType() == typeof(BoxCollider))
                            held.GetComponent<BoxCollider>().size = new Vector3(10, 10, 10);

                        held.layer = LayerMask.NameToLayer("Moving");
                        held.transform.parent = grabPoint.transform;
                        held.transform.localPosition = Vector3.zero;
                    }
                }
                else
                {
                    
                }
            }
            else
            {
                Magnetism_Movable grabbed = held.GetComponent<Magnetism_Movable>();
                StartCoroutine(grabbed.LerpScale(false));
                grabbed.grabbable = true;
                held.GetComponent<Rigidbody>().useGravity = true;
                held.GetComponent<Rigidbody>().mass = 1;
                Collider c = held.GetComponent<Collider>();
                if (c.GetType() == typeof(SphereCollider))
                    held.GetComponent<SphereCollider>().radius = 0.5f;
                else if (c.GetType() == typeof(BoxCollider))
                    held.GetComponent<BoxCollider>().size = new Vector3(5, 5, 5);
                held.layer = LayerMask.NameToLayer("Moveable");
                held.transform.parent = null;
                held = null;
            }
        }

        void Grab()
        {
            if (held != null)
            {
                Rigidbody rb = held.GetComponent<Rigidbody>();
                held.transform.localPosition = Vector3.zero;//Vector3.Lerp(held.transform.position, Camera.main.transform.forward * 3 + Camera.main.transform.position, Time.fixedDeltaTime * MoveSpeed);
                held.transform.localRotation = Quaternion.Euler(Vector3.up + held.transform.localRotation.eulerAngles);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        public void OnPolarize(InputValue value)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, polarizeDistance))
            {
                if (InvokePolarize != null)
                    InvokePolarize(this.gameObject, hit.collider.gameObject);
            }
        }

        public void OnPolarityChange(InputValue value)
        {
            polarity *= -1;
        }

        public IEnumerator TemporaryDisable(float time)
        {
            Debug.Log("called");
            this.enabled = false;
            yield return new WaitForSeconds(time);
            this.enabled = true;
        }
    }
}