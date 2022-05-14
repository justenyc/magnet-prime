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
    
    public void StateStart()
    {
        InitializePatrolPoints();
        manager.agent.destination = manager.points[manager.patrolPointIndex].position;
    }

    public void StateUpdate()
    {
        manager.agent.destination = manager.points[manager.patrolPointIndex].position;

        if (!manager.myMagnetism.beingMagnetized)
            manager.rb.velocity = Vector3.zero;

        if (manager.patrolling)
            manager.agent.speed = manager.agentSpeed;
        else
            manager.agent.speed = 0;

        Sight(manager.head.forward, manager.sightDistance);
        Sight(manager.head.forward + manager.transform.right * manager.sightModifier, manager.sightDistance);
        Sight(manager.head.forward + manager.transform.right * -manager.sightModifier, manager.sightDistance);
        //Debug.DrawRay(manager.head.position, manager.head.forward * manager.sightDistance + manager.transform.right * manager.sightModifier, Color.magenta);
        //Debug.DrawRay(manager.head.position, (manager.head.forward + manager.transform.right * manager.sightModifier) * manager.sightDistance, Color.magenta);
    }

    private void Sight(Vector3 direction, float distance)
    {
        if (!manager.myMagnetism.beingMagnetized)
        {
            Debug.DrawRay(manager.head.position, direction * distance, Color.green);
            manager.sightLight.SetActive(true);

            RaycastHit hit;
            if (Physics.Raycast(manager.head.position, direction, out hit, distance, manager.mask))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.tag == "Player")
                {
                    manager.currentState = new IAlert(manager, hit.collider.transform);
                }
            }
        }
        else
        {
            manager.sightLight.SetActive(false);
        }
    }

    private void Sight(Vector3 direction, float distance, float angle)
    {
        if (!manager.myMagnetism.beingMagnetized)
        {
            Debug.DrawRay(manager.head.position, direction * distance, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(manager.head.position, direction, out hit, distance, manager.mask))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }

    public void InitializePatrolPoints()
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PatrolPoint"))
        {
            manager.patrolPointIndex++;

            if (manager.patrolPointIndex == manager.points.Count)
                manager.patrolPointIndex = 0;

            manager.agent.destination = manager.points[manager.patrolPointIndex].position;
        }
    }
}
