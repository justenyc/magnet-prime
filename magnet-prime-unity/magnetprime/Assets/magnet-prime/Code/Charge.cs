using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Charge : MonoBehaviour
{
    [SerializeField] FirstPersonController player;
    [SerializeField] int charge = 0;
    [SerializeField] int chargeStrength = 0;
    [SerializeField] int minimumCharge = 5;
    [SerializeField] int overChargeLimit = 10;

    [SerializeField] bool overChargeDecay = true;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError(this.name + " Says: Player not found > Using FindObjectOfType<FirstPersonController>()");
            player = FindObjectOfType<FirstPersonController>();
            Debug.Log("Player is now " + player);
        }

        player.InvokeShoot += ShootListener;
    }

    public int GetCharge()
    {
        return charge;
    }

    public int GetChargeStrength()
    {
        return chargeStrength;
    }

    private void OnDestroy()
    {
        player.InvokeShoot -= ShootListener;
    }

    void OverCharge()
    {
        if (chargeStrength > 0)
        {
            Debug.Log("Positive Over Charge");
            chargeStrength = 1;
        }
        else if (chargeStrength < 0)
        {
            Debug.Log("Negative Over Charge");
            chargeStrength = -1;
        }

        if (overChargeDecay)
            overChargeLimit += Mathf.RoundToInt(overChargeLimit / 4);
    }

    void ShootListener(GameObject objectHit)
    {
        //Debug.Log(objectHit);
        if (objectHit == this.gameObject)
        {
            if (player.lastShot)
            {
                chargeStrength++;

                if (chargeStrength >= minimumCharge)
                    charge = 1;
            }
            else
            {
                chargeStrength--;

                if (chargeStrength <= -minimumCharge)
                    charge = -1;
            }

            if (chargeStrength == 0)
                charge = 0;

            if (Mathf.Abs(chargeStrength) >= overChargeLimit)
                OverCharge();
        }
    }
}
