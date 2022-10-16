using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Charge : MonoBehaviour
{
    [SerializeField] FirstPersonController player;
    [SerializeField] int polarity = 0;
    [SerializeField] int chargeStrength = 0;
    [SerializeField] int minimumCharge = 5;
    [SerializeField] int overChargeLimit = 10;

    [SerializeField] bool overChargeDecay = true;
    public Action<int> polarityChange;
    [SerializeField] Material mainMaterial;

    private void Start()
    {
        mainMaterial = mainMaterial == null ? this.GetComponent<MeshRenderer>().material : mainMaterial;
        ChargeHandler();

        player = player == null ? FindObjectOfType<FirstPersonController>() : player;
        player.InvokeShoot += ShootListener;

        if (polarityChange != null)
        {
            //Debug.Log("Post of polarityChange by " + this.name);
            polarityChange(polarity);
        }
    }

    public int GetPolarity()
    {
        return polarity;
    }

    public int GetChargeStrength()
    {
        return chargeStrength;
    }

    private void OnDestroy()
    {
        player.InvokeShoot -= ShootListener;
    }

    public void RandomizePolarity()
    {
        float randomNum = UnityEngine.Random.Range(-1, 2);

        if(randomNum > 0)
        {
            chargeStrength *= -1;
        }
        if(randomNum == 0)
        {
            RandomizePolarity();
        }
        ChargeHandler();
    }

    void OverCharge()
    {
        if (chargeStrength > 0)
        {
            chargeStrength = 1;
        }
        else if (chargeStrength < 0)
        {
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
                chargeStrength = Mathf.Clamp(chargeStrength, -5, 5);
            }
            else
            {
                chargeStrength--;
                chargeStrength = Mathf.Clamp(chargeStrength, -5, 5);
            }

            ChargeHandler();

            if (Mathf.Abs(chargeStrength) >= overChargeLimit)
                OverCharge();
        }
    }

    void SetFresnelColor(Color c)
    {
        mainMaterial = mainMaterial == null ? this.GetComponent<MeshRenderer>().material : mainMaterial;
        mainMaterial.SetColor("Color_Fresnel", c);
    }

    void ChargeHandler()
    {
        if (chargeStrength >= minimumCharge)
        {
            polarity = 1;
            SetFresnelColor(Color.red);
            if (polarityChange != null)
                polarityChange(polarity);
        }
        else if (chargeStrength <= -minimumCharge)
        {
            polarity = -1;
            SetFresnelColor(Color.blue);
            if (polarityChange != null)
                polarityChange(polarity);
        }
        else if (chargeStrength == 0)
        {
            polarity = 0;
            SetFresnelColor(Color.black);
            if (polarityChange != null)
                polarityChange(polarity);
        }
    }
}
