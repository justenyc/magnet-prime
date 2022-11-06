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
    public Action<float> TakeDamageAction;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeUntilRegenCD > 0)
            timeUntilRegenCD -= Time.fixedDeltaTime;
        else
        {
            currHealth += regenRate * Time.fixedDeltaTime;
            currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        }
        timeUntilRegenCD = Mathf.Clamp(timeUntilRegenCD, 0, timeUntilRegen);

        if (TakeDamageAction != null)
            TakeDamageAction(currHealth / maxHealth);
    }

    public void TakeDamage(float amount)
    {
        timeUntilRegenCD = timeUntilRegen;
        currHealth -= amount;

        if (currHealth < 0)
        {
            Die();
            return;
        }

        SfxManager.instance.RandomizePitch(SfxManager.instance.mainSource, 0.9f, 1.2f);
        SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "PlayerDamage");
    }

    public void Die()
    {
        SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "PlayerDie");

        if (DieAction != null)
            DieAction();
    }
}
