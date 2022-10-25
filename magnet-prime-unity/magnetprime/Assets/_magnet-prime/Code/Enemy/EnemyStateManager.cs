using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [Header("Set Objects")]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform head;
    public GameObject sightLight;
    public GameObject projectile;
    public Rigidbody rb;
    public AudioSource aSource;

    [Header("Patrol Properties")]
    public float agentSpeed = 3.5f;
    public float sightDistance;
    public float sightModifier = 10f;
    public LayerMask mask;
    public Transform patrolPointHolder;
    [Tooltip("How long it takes for the eney to transition from the magnetized state to another state")]
    public float magnetizedTransitionTime = 2f;
    public int patrolPointIndex { get; set; } = 0;
    public bool patrolling = false;
    public List<Transform> points { get; set; } = new List<Transform>();

    [Space(10)]
    public float fireRate = 1f;
    public float fireRateCountdown { get; set; }
    public IEnemyState currentState;
    [HideInInspector] public Transform playerPosition { get; set; }

    public Magnetism_Movable myMagnetism { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        myMagnetism = this.GetComponent<Magnetism_Movable>();
        fireRateCountdown = fireRate;
        aSource = this.GetComponent<AudioSource>();
        InitializePatrolPoints();

        currentState = new IPatrolling(this);
        currentState.StateStart();

        if (!head)
        {
            Debug.LogError("Head not found. Please check the inspector for " + this.name);
            Debug.Break();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentState.StateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(collision);
    }

    public void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    public void FireProjectile(Transform target)
    {
        Projectile newProj = Instantiate(projectile, transform.position + transform.forward, transform.rotation).GetComponent<Projectile>();
        newProj.target = target;
        fireRateCountdown = fireRate;
        SfxManager.instance.RandomizePitch(aSource, 1, 1.5f);
        SfxManager.instance.PlayFromSource(aSource, "Enemy_Projectile");
    }

    void InitializePatrolPoints()
    {
        if (patrolPointHolder != null)
        {
            foreach (Transform t in patrolPointHolder)
            {
                points.Add(t);
                continue;
            }
        }
        else
        {
            Debug.LogError("PatrolPoints in " + this.name + " were not found. Please check the inspector");
            Debug.Break();
        }
    }
}