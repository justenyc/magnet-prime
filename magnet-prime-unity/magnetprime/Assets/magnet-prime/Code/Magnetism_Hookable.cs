using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[RequireComponent(typeof(Charge))]
public class Magnetism_Hookable : Magnetism
{
    public Charge myCharge;
    public Action<int> polarityChange;

    public float polarizeCD = 5f;
    public float polarizeCDTime;
    float polarizeStrength = 1;

    FirstPersonController player;

    // Start is called before the first frame update
    void Start()
    {
        if (myCharge == null)
        {
            Debug.LogError(this.name + " Says: myCharge not found > Using GetComponent<Charge>()");
            myCharge = this.GetComponent<Charge>();
            Debug.Log("myCharge is now " + myCharge);
        }

        player = FindObjectOfType<FirstPersonController>();
        player.InvokePolarize += OnPlayerPolarize;
    }

    // Update is called once per frame
    void Update()
    {
        if (polarizeCDTime > 0)
        {
            polarizeCDTime = Mathf.Clamp(polarizeCDTime - Time.fixedDeltaTime, 0, polarizeCD);
        }
    }

    public override void OnPlayerPolarize(GameObject player, GameObject objectHit)
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        int action = fpc.polarity * myCharge.GetPolarity();


        /*if (objectHit == this.gameObject)
        {
            if (fpc.polarity * myCharge.GetPolarity() < 0)
            {
                polarizeStrength = fpc.polarizeStrength;
                polarizeCDTime = polarizeCD;
            }
            else if (fpc.polarity * myCharge.GetPolarity() > 0)
            {
                //Collider[] collidersInRange = 
            }
        }*/
    }
}
