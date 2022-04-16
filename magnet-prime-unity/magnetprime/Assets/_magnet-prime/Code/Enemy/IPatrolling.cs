using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPatrolling : IEnemyState
{
    EnemyStateManager manager;

    public IPatrolling(EnemyStateManager managerRef)
    {
        manager = managerRef;
    }
    
    // Start is called before the first frame update
    public void StateStart()
    {
        InitializePatrolPoints();
        manager.agent.destination = manager.points[manager.patrolPointIndex].position;
    }

    // Update is called once per frame
    public void StateUpdate()
    {
        
    }

    void InitializePatrolPoints()
    {
        if (manager.patrolPointHolder != null)
        {
            foreach (Transform t in manager.patrolPointHolder)
            {
                manager.points.Add(t);
            }
        }
        else
        {
            Debug.LogError("PatrolPoints in " + manager.name + " were not found. Please check the inspector");
            Debug.Break();
        }
    }
}
