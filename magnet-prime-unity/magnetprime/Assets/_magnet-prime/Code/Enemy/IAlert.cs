using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAlert : IEnemyState
{
    EnemyStateManager manager;
    Transform player;

    public IAlert(EnemyStateManager managerRef, Transform playerRef)
    {
        manager = managerRef;
        player = playerRef;
        StateStart();
    }

    // Start is called before the first frame update
    public void StateStart()
    {
        manager.fireRateCountdown = manager.fireRate;
        manager.agent.speed = 0;
        manager.animator.SetBool("Alert", true);
        SfxManager.instance.PlayFromSource(manager.aSource, "Enemy_Alert");
    }

    // Update is called once per frame
    public void StateUpdate()
    {
        manager.transform.LookAt(player.transform);

        TrackPlayer();
    }

    void TrackPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(manager.head.position, player.position - manager.head.position, out hit, Mathf.Infinity, manager.mask))
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Player")
            {
                manager.playerPosition = hit.transform;
            }
            else
            {
                manager.playerPosition = null;
                ChangeState(new IPatrolling(manager));
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnCollisionEnter(Collision collision)
    {

    }

    public void ChangeState(IEnemyState newState)
    {
        manager.animator.SetBool("Alert", false);
        manager.currentState = newState;
    }
}
