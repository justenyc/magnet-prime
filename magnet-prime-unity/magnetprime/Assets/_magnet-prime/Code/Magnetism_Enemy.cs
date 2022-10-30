using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]


public class Magnetism_Enemy : Magnetism_Movable
{
    public bool randomizePolarity = true;
    public bool reboot { get; private set; } = true;
    private void Start()
    {
        if(randomizePolarity == true)
        {
            myCharge.RandomizePolarity();
            //base.OnPolarityChange(myCharge.GetPolarity());
        }
    }

    private void FixedUpdate()
    {
        
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

    public override void ApplyForce(int magnetism, Vector3 direction)
    {
        if (reboot && beingMagnetized) { reboot = magnetism >= 0 ? true : false; }
        if (magnetism != 0)
        {
            beingMagnetized = true;
            rigidBody.useGravity = false;
            rigidBody.AddForce(direction = magnetism > 0 ? direction * 2 : direction, ForceMode.Force);
        }
        else
        {
            beingMagnetized = false;
            rigidBody.useGravity = true;
        }
    }

    void OnEnable()
    {
        Start();
    }
}
