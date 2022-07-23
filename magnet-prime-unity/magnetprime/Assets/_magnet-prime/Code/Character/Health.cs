using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float timeUntilRegen = 2f;
    public float timeUntilRegenCD;
    public float regenRate = 1f;
    public float maxHealth = 100f;
    public float currHealth = 100f;

    public Action DieAction;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
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

        if (currHealth < 0)
            Die();
    }

    public void Die()
    {
        if (DieAction != null)
            DieAction();
    }
}
