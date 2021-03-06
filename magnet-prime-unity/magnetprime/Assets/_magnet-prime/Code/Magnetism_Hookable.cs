using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using DG.Tweening;

[RequireComponent(typeof(Charge))]
public class Magnetism_Hookable : Magnetism
{
    public Charge myCharge;
    public Action<int> polarityChange;
    public float hookSpeed = 1;
    public float polarizeCD = 5f;
    public float polarizeCDTime;
    float polarizeStrength = 250;

    public Collider[] nearColliders;

    FirstPersonController player;

    // Start is called before the first frame update
    void Start()
    {
        myCharge = this.GetComponent<Charge>() ?? myCharge;

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

        if (objectHit == this.gameObject)
        {
            if (fpc.polarity * myCharge.GetPolarity() < 0)
            {
                fpc.transform.DOMove(transform.position, hookSpeed, false);
                StartCoroutine(fpc.TemporaryDisable(hookSpeed));
                polarizeCDTime = polarizeCD;
            }
            else if (fpc.polarity * myCharge.GetPolarity() > 0)
            {
                Debug.Log("getting near colliders");
                nearColliders = Physics.OverlapSphere(transform.position, 10);
                foreach(Collider col in nearColliders)
                {
                    Magnetism_Movable temp = col.GetComponent<Magnetism_Movable>();
                    if(temp != null)
                    {
                        int tempAction = myCharge.GetPolarity() * temp.GetPolarity();
                        col.attachedRigidbody.AddExplosionForce(tempAction * myCharge.GetChargeStrength() * polarizeStrength, transform.position, 10);
                    }
                }
                polarizeCDTime = polarizeCD;
            }
        }
    }
}
