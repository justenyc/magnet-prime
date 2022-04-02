using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switchActive;
    public Action SwitchAction;
    public LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        switchActive = true;
        if (SwitchAction != null)
            SwitchAction();
        Debug.Log(switchActive);
    }

    private void OnTriggerExit(Collider other)
    {
        switchActive = false;
        if (SwitchAction != null)
            SwitchAction();
        Debug.Log(switchActive);
    }
}
