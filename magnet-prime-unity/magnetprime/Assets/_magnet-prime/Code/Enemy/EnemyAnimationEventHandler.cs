using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public AudioSource aSource;

    private void Start()
    {
        aSource = GetComponentInParent<AudioSource>();
    }

    public void FireProjectile()
    {
        EnemyStateManager esm = GetComponentInParent<EnemyStateManager>();
        esm.FireProjectile(esm.playerPosition);
    }

    public void PlayFootStepSound()
    {
        SfxManager.instance.ResetSourceSettings(aSource);
        SfxManager.instance.PlayFromSource(aSource, "Footstep_Enemy");
    }
}
