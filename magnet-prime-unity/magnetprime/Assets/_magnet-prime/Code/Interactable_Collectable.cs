using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Interactable_Collectable : Interactable
{
    public override void Interact()
    {
        Inventory.instance.AddItem(this);
        //this.gameObject.SetActive(false);
        UiManager.instance.EnableInteractMessage(false);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        FirstPersonController fpc = other.GetComponentInChildren<FirstPersonController>();
        if (fpc)
        {
            fpc.Interact += Interact;
            UiManager.instance.EnableInteractMessage(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FirstPersonController fpc = other.GetComponentInChildren<FirstPersonController>();
        if (fpc)
        {
            fpc.Interact -= Interact;
            UiManager.instance.EnableInteractMessage(false);
        }
    }
}
