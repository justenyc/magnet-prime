using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]


public class Magnetism_Enemy : Magnetism_Movable
{
    public bool randomizePolarity = true;
    private void Start()
    {
        if(randomizePolarity == true)
        {
            myCharge.RandomizePolarity();
            //base.OnPolarityChange(myCharge.GetPolarity());
        }
    }

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



    void OnEnable()
    {
        Start();
    }
}
