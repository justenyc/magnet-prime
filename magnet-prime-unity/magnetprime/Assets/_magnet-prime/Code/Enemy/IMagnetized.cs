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
        manager.radiusDetector.SetActive(false);
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
            if (manager.myMagnetism.reboot)
            {
                if (stateChangeCounter > 0)
                    stateChangeCounter -= Time.deltaTime;
                else
                    ChangeState(new IPatrolling(manager));
            }
        }
        //Debug.Log($"IMagnetized says: stateChangeCounter = {stateChangeCounter}");
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"{collision.collider.name}, {collision.impulse.magnitude}");
        if (collision.impulse.magnitude > 10f)
        {
            float vol = 0.25f * manager.rb.velocity.magnitude;
            SfxManager.instance.SetVolume(manager.aSource, Mathf.Clamp(vol, 0.25f, 1));
            SfxManager.instance.RandomizePitch(manager.aSource, 0.1f, 0.5f);
            SfxManager.instance.PlayFromSource(manager.aSource, "Box_clang", oneshot: true);
        }
        else if (collision.impulse.magnitude > 5f)
        {
            float vol = 0.25f * manager.rb.velocity.magnitude;
            SfxManager.instance.SetVolume(manager.aSource, Mathf.Clamp(vol, 0.25f, 1));
            SfxManager.instance.RandomizePitch(manager.aSource, 1f, 1.2f);
            SfxManager.instance.PlayFromSource(manager.aSource, "Box_clang", oneshot: true);
        }
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void ChangeState(IEnemyState newState)
    {
        manager.GetComponentInChildren<ParticleSystem>().Stop();
        manager.currentState = newState;
    }
}
