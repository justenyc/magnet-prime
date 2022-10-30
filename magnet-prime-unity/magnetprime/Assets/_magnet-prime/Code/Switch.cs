using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool switchActive;
    public Action SwitchAction;
    
    [SerializeField] HashSet<Collider> colliders = new HashSet<Collider>();
    [SerializeField] Material myMat;

    private void Start()
    {
        myMat = this.GetComponentsInChildren<MeshRenderer>()[1].material;
        myMat.EnableKeyword("_EMISSION");
    }

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
            myMat.SetColor("_EmissionColor", Color.green);
        }
        else
        {
            switchActive = false;
            myMat.SetColor("_EmissionColor", Color.red);
        }

        if (SwitchAction != null)
            SwitchAction();
    }
}
