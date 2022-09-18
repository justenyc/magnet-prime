using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMagnetized : IEnemyState
{
    EnemyStateManager manager;
    float stateChangeCounter = 2f;

    public IMagnetized(EnemyStateManager managerRef)
    {
        manager = managerRef;
        StateStart();
    }

    // Start is called before the first frame update
    public void StateStart()
    {
        manager.sightLight.SetActive(false);
        manager.agent.enabled = false;
        stateChangeCounter = manager.magnetizedTransitionTime;
        manager.GetComponentInChildren<ParticleSystem>().Play();
    }

    // Update is called once per frame
    public void StateUpdate()
    {
        if (manager.myMagnetism.beingMagnetized)
        {
            manager.animator.SetBool("Magnetized", manager.myMagnetism.beingMagnetized);
            stateChangeCounter = manager.magnetizedTransitionTime;
        }
        else
        {
            if (stateChangeCounter > 0)
                stateChangeCounter -= Time.deltaTime;
            else
                ChangeState(new IPatrolling(manager));
        }
        //Debug.Log($"IMagnetized says: stateChangeCounter = {stateChangeCounter}");
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ChangeState(IEnemyState newState)
    {
        manager.GetComponentInChildren<ParticleSystem>().Stop();
        manager.currentState = newState;
    }
}
