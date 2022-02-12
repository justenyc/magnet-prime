using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
public class Magnetism_Immovable : Magnetism
{
    public Charge myCharge;
    public float magnetismStrength = 1;
    public List<Magnetism_Movable> movableObjectsWithCharge;

    private void Start()
    {
        if (myCharge == null)
        {
            Debug.LogError(this.name + " Says: myCharge not found > Using GetComponent<Charge>()");
            myCharge = this.GetComponent<Charge>();
            Debug.Log("myCharge is now " + myCharge);
        }

        //movableObjectsWithCharge = new List<Magnetism_Movable>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int ii = 0; ii < movableObjectsWithCharge.Count; ii++)
        {
            Magnetism_Movable temp = movableObjectsWithCharge[ii];
            int magnetism = MagnetismAction(temp.GetPolarity());

            if (magnetism != 0)
            {
                Rigidbody tempRb = temp.GetComponent<Rigidbody>();
                tempRb.AddForce(targetDirection(temp.transform.position).normalized * magnetismStrength * Mathf.Abs(myCharge.GetChargeStrength()) / 10 * magnetism, ForceMode.Force);
            }
        }
    }

    public void AddMovable(Magnetism_Movable movable)
    {
        try
        {
            movableObjectsWithCharge.Remove(movable);
            movableObjectsWithCharge.Add(movable);
        }
        catch
        {
            movableObjectsWithCharge.Add(movable);
        }
    }

    Vector3 targetDirection(Vector3 target)
    {
        return target - this.transform.position;
    }

    int MagnetismAction(int otherCharge)
    {
        int myCurrentCharge = myCharge.GetPolarity();
        int action = myCurrentCharge * otherCharge;

        return action;
    }

    public override void OnPlayerPolarize(GameObject player, GameObject objectHit)
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc.polarity * myCharge.GetPolarity() != 0)
        {
            if(objectHit != this.gameObject)
            {
                Magnetism_Movable temp = objectHit.GetComponent<Magnetism_Movable>();
                foreach(Magnetism_Movable mm in movableObjectsWithCharge)
                {
                    if (mm == temp)
                        movableObjectsWithCharge.Remove(mm);
                }
            }
            else if (objectHit == this.gameObject)
            {
                int action = MagnetismAction(player.GetComponent<FirstPersonController>().polarity);
                //**WIP**
            }
        }
    }
}
