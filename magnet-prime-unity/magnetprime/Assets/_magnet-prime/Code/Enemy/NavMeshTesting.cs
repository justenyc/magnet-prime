using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTesting : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform patrolPoints;
    public int patrolDestination = 0;
    public List<Transform> points;

    // Start is called before the first frame update
    void Start()
    {
        InitializePatrolPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializePatrolPoints()
    {
        if (patrolPoints != null)
        {
            foreach (Transform t in patrolPoints)
            {
                points.Add(t);
                
            }
            patrolPoints.parent = null;
        }
        else
        {
            Debug.Break();
            Debug.LogError("PatrolPoints in " + this.name + " were not found. Please check the inspector");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PatrolPoint"))
        {

        }
    }

    void PatrolPointHandler()
    {
        try
        {
            agent.destination = points[patrolDestination].position;
        }
        catch
        {
            patrolDestination = 0;
            agent.destination = points[patrolDestination].position;
        }
    }
}
