using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [Header("Set Objects")]
    public NavMeshAgent agent;
    public Transform head;
    public GameObject sightLight;
    public GameObject projectile;
    public Rigidbody rb;

    [Header("Patrol Properties")]
    public float agentSpeed = 3.5f;
    public float sightDistance;
    public float sightModifier = 10f;
    public LayerMask mask;
    public Transform patrolPointHolder;
    [Tooltip("How long it takes for the eney to transition from the magnetized state to another state")]
    public float magnetizedTransitionTime = 2f;
    [HideInInspector] public int patrolPointIndex = 0;
    [HideInInspector] public bool patrolling = false;
    [HideInInspector] public List<Transform> points;

    [Space(10)]
    public float fireRate = 1f;
    [HideInInspector] public float fireRateCountdown;
    public IEnemyState currentState;

    [HideInInspector] public Magnetism_Movable myMagnetism;
    // Start is called before the first frame update
    void Start()
    {
        myMagnetism = this.GetComponent<Magnetism_Movable>();
        fireRateCountdown = fireRate;

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
}
