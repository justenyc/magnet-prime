using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
public class Magnetism_Immovable : MonoBehaviour
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

        movableObjectsWithCharge = new List<Magnetism_Movable>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int ii = 0; ii < movableObjectsWithCharge.Count; ii++)
        {
            Magnetism_Movable temp = movableObjectsWithCharge[ii];
            int magnetism = Magnetism(temp.GetCharge());
            Debug.Log(magnetism);
            switch (magnetism)
            {
                case 0:
                    break;

                case 1:
                    Rigidbody tempRb = temp.GetComponent<Rigidbody>();
                    tempRb.AddForce(targetDirection(temp.transform.position).normalized * magnetismStrength * myCharge.GetChargeStrength() / 10, ForceMode.Force);
                    break;

                case -1:
                    Rigidbody tempRbPull = temp.GetComponent<Rigidbody>();
                    tempRbPull.AddForce(-targetDirection(temp.transform.position).normalized * magnetismStrength * myCharge.GetChargeStrength() / 10, ForceMode.Force);
                    break;

                default:
                    break;
            }
        }
    }

    Vector3 targetDirection(Vector3 target)
    {
        return target - this.transform.position;
    }

    int Magnetism(int otherCharge)
    {
        int myCurrentCharge = myCharge.GetCharge();
        int action = myCurrentCharge + otherCharge;

        if (myCurrentCharge != 0 && otherCharge != 0)
        {
            switch (action)
            {
                case 2:
                    return 1;

                case -2:
                    return 1;

                case 0:
                    return -1;

                default:
                    return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}
