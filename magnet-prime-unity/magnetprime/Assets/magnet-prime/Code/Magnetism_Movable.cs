using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]
public class Magnetism_Movable : Magnetism
{
    //**WIP** Polarize actions
    public Charge myCharge;
    public bool grabbable;
    public Vector3 direction;
    public Rigidbody rigidBody;
    public float polarizeCD = 5f;
    public float polarizeCDTime;
    public bool dragToPlayer = false;

    private void Start()
    {
        polarizeCDTime = polarizeCD;
        if (myCharge == null)
        {
            Debug.LogError(this.name + " Says: myCharge not found > Using GetComponent<Charge>()");
            myCharge = this.GetComponent<Charge>();
            Debug.Log("myCharge is now " + myCharge);
        }

        if (rigidBody == null)
        {
            Debug.LogError(this.name + " Says: RigidBody not found > Using GetComponent<RigidBody>()");
            rigidBody = this.GetComponent<Rigidbody>();
            Debug.Log("RigidBody is now " + myCharge);
        }

        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        player.InvokePolarize += OnPlayerPolarize;
    }

    private void FixedUpdate()
    {
        if (polarizeCDTime > 0)
        {
            polarizeCDTime = Mathf.Clamp(polarizeCDTime - Time.fixedDeltaTime, 0, polarizeCD);
        }
        else
        {
            dragToPlayer = false;
        }
    }

    public void SetTargetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    public void SetTargetDirection(Vector3 newDirection, float modifer)
    {
        direction = newDirection * modifer;
    }

    public int GetPolarity()
    {
        return myCharge.GetPolarity();
    }

    public override void OnPlayerPolarize(GameObject player, GameObject objectHit)
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        int action = fpc.polarity * myCharge.GetPolarity();

        if (fpc.polarity * myCharge.GetPolarity() < 0)
        {
            polarizeCDTime = polarizeCD;
        }
        else if (fpc.polarity * myCharge.GetPolarity() > 0)
        {
            rigidBody.AddForce(transform.position - player.transform.position, ForceMode.Impulse);
        }
    }
}
