using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Health : MonoBehaviour
{
    public float timeUntilRegen = 2f;
    public float timeUntilRegenCD;
    public float regenRate = 1f;
    public float maxHealth = 100f;
    public float currHealth = 100f;

    public Action DieAction;
    public Action TakeDamageAction;

    public camerashaker camShake;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeUntilRegenCD > 0)
            timeUntilRegenCD -= Time.deltaTime;
        else
        {
            currHealth += regenRate * Time.deltaTime;
            currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        }
        timeUntilRegenCD = Mathf.Clamp(timeUntilRegenCD, 0, timeUntilRegen);
    }

    public void TakeDamage(float amount)
    {
        timeUntilRegenCD = timeUntilRegen;
        currHealth -= amount;

        SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "PlayerDamage");
        camShake.ShakeCamera(2.5f, .2f);
        

        if (TakeDamageAction != null)
            TakeDamageAction();

        if (currHealth < 0)
            Die();
    }

    public void Die()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<FirstPersonController>().enabled = false;
        if (DieAction != null)
            DieAction();
        currHealth = maxHealth;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<FirstPersonController>().enabled = true;
    }
}
