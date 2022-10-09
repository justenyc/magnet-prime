using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switchActive;
    public Action SwitchAction;
    public LayerMask layerMask;
    [SerializeField] HashSet<Collider> colliders = new HashSet<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        colliders.Add(other);
        SwitchActive();
    }

    private void OnTriggerExit(Collider other)
    {
        colliders.Remove(other);
        SwitchActive();
    }

    void SwitchActive()
    {
        if (colliders.Count > 0)
        {
            switchActive = true;
        }
        else
        {
            switchActive = false;
        }

        if (SwitchAction != null)
            SwitchAction();
    }
}
