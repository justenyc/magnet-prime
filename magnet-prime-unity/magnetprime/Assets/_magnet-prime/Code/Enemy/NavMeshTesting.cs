using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTesting : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform head;
    public float agentSpeed = 3.5f;
    public int patrolPointIndex = 0;
    public Transform patrolPointHolder;
    public bool patrolling = false;
    public List<Transform> points;

    // Start is called before the first frame update
    void Start()
    {
        InitializePatrolPoints();
        agent.destination = points[patrolPointIndex].position;
        
        if(!head)
        {
            Debug.LogError("Head not found please check the inspector for " + this.name);
            Debug.Break();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolling)
            agent.speed = agentSpeed;
        else
            agent.speed = 0;

        Debug.DrawRay(transform.position, head.forward * 10, Color.cyan);
    }

    void InitializePatrolPoints()
    {
        if (patrolPointHolder != null)
        {
            foreach (Transform t in patrolPointHolder)
            {
                points.Add(t);
            }
        }
        else
        {
            Debug.LogError("PatrolPoints in " + this.name + " were not found. Please check the inspector");
            Debug.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PatrolPoint"))
        {
            patrolPointIndex++;

            if (patrolPointIndex == points.Count)
                patrolPointIndex = 0;
            
            agent.destination = points[patrolPointIndex].position;
        }
    }
}
