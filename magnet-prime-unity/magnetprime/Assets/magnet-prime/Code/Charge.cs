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
    public delegate void ChargeDelegate(int charge);
    public event ChargeDelegate ChargeEvent;
    [SerializeField] Material mainMaterial;

    private void Start()
    {
        if (mainMaterial == null)
        {
            Debug.LogError(this.name + " Says: Main Material not found > Using GetComponent<>()");
            mainMaterial = this.GetComponent<MeshRenderer>().material;
            Debug.Log("Main Material is now " + mainMaterial);
            ChargeHandler();
        }
        ChargeHandler();

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
        if (objectHit == this.gameObject)
        {
            if (player.lastShot)
            {
                chargeStrength++;
            }
            else
            {
                chargeStrength--;
            }

            ChargeHandler();

            if (Mathf.Abs(chargeStrength) >= overChargeLimit)
                OverCharge();
        }
    }

    void SetFresnelColor(Color c)
    {
        mainMaterial.SetColor("Color_Fresnel", c);
    }

    void ChargeHandler()
    {
        if (chargeStrength >= minimumCharge)
        {
            charge = 1;
            SetFresnelColor(Color.red);
        }
        else if (chargeStrength <= -minimumCharge)
        {
            charge = -1;
            SetFresnelColor(Color.blue);
        }
        else if (chargeStrength == 0)
        {
            charge = 0;
            SetFresnelColor(Color.black);
        }
    }
}