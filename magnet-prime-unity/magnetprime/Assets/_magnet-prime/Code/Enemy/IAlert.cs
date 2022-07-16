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
                //Debug.Log("Locked onto player");
                FireProjectile(hit.transform);
            }
            else
            {
                manager.currentState = new IPatrolling(manager);
            }
        }
    }

    void FireProjectile(Transform target)
    {
        if (manager.fireRateCountdown > 0)
        {
            manager.fireRateCountdown -= Time.deltaTime;
        }
        else
        {
            Projectile newProj = GameObject.Instantiate(manager.projectile, manager.transform.position + manager.transform.forward, manager.transform.rotation).GetComponent<Projectile>();
            newProj.target = target;
            manager.fireRateCountdown = manager.fireRate;
            SfxManager.instance.RandomizePitch(manager.aSource, 1, 1.5f);
            SfxManager.instance.PlayFromSource(manager.aSource, "Enemy_Projectile");
        }
    }

    public void OnTriggerEnter(Collider other)
    {

    }
}
