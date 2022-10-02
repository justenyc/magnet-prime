using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]
public class Magnetism_Enemy : Magnetism_Movable
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<EnemyStateManager>().OnCollisionEnter(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("BoxForceField") || other.gameObject.layer == LayerMask.NameToLayer("AbsoluteDeath"))
        {
            Destroy(this.gameObject);
        }
    }
}
