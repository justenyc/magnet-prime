using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public void FireProjectile()
    {
        EnemyStateManager esm = GetComponentInParent<EnemyStateManager>();
        esm.FireProjectile(esm.playerPosition);
    }
}
