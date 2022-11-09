using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPatrolling : IEnemyState
{
    EnemyStateManager manager;

    public IPatrolling(EnemyStateManager managerRef)
    {
        manager = managerRef;
        StateStart();
    }

    public void StateStart()
    {
        manager.sightLight.SetActive(true);
        manager.radiusDetector.SetActive(true);
        manager.agent.enabled = true;
        manager.agent.destination = manager.points[manager.patrolPointIndex].position;
        manager.animator.SetBool("Magnetized", manager.myMagnetism.beingMagnetized);
    }

    public void StateUpdate()
    {
        if (manager.patrolling)
        {
            manager.agent.speed = manager.agentSpeed;
        }
        else
        {
            manager.agent.speed = 0;
        }

        manager.animator.SetFloat("MoveSpeed", manager.agentSpeed);

        if (manager.myMagnetism.beingMagnetized)
            manager.currentState = new IMagnetized(manager);

        manager.rb.velocity = Vector3.zero;
    }

    private void Sight(Vector3 direction, float distance)
    {
        Debug.DrawRay(manager.head.position, direction * distance, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(manager.head.position, direction, out hit, distance, manager.mask))
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Player")
            {
                manager.currentState = new IAlert(manager, hit.collider.transform);
            }
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

    public void OnTriggerEnter(Collider other)
    {
        manager.rb.velocity = Vector3.zero;
        manager.rb.angularVelocity = Vector3.zero;

        if (other.CompareTag("PatrolPoint"))
        {
            manager.patrolPointIndex++;

            if (manager.patrolPointIndex == manager.points.Count)
                manager.patrolPointIndex = 0;

            manager.agent.destination = manager.points[manager.patrolPointIndex].position;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Magnetism_Movable mm = collision.collider.GetComponent<Magnetism_Movable>();
        if(collision.collider.TryGetComponent(out Rigidbody rb) && mm)
        {
            //Debug.Log(rb.velocity.magnitude);
            if (rb.velocity.magnitude < 10)
                rb.velocity = (Vector3.up + new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)) * 10);
            else
                manager.myMagnetism.beingMagnetized = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 direction = other.transform.position - manager.head.transform.position;
            Debug.DrawRay(manager.head.transform.position, direction * 10, Color.magenta);
            Sight(direction, manager.sightDistance);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        manager.currentState = newState;
    }
}
