using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]
public class Magnetism_Movable : MonoBehaviour
{
    public Charge myCharge;

    private void Start()
    {
        if (myCharge == null)
        {
            Debug.LogError(this.name + " Says: myCharge not found > Using GetComponent<Charge>()");
            myCharge = this.GetComponent<Charge>();
            Debug.Log("myCharge is now " + myCharge);
        }
    }

    public int GetCharge()
    {
        return myCharge.GetCharge();
    }
}