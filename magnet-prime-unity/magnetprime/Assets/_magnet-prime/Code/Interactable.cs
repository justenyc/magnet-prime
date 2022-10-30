using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

public class Interactable : MonoBehaviour
{
    public GameObject playerGun = null;
    public GameObject pedastalGun = null;
    public UnityEvent customEvent;
    bool interact = true;

    public virtual void Interact()
    {
        customEvent.Invoke();
    }

    public void EnableGun()
    {
        playerGun?.SetActive(true);
        pedastalGun?.SetActive(false);
        FindObjectOfType<FirstPersonController>().hasGun = true;
        customEvent.RemoveAllListeners();
        SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyPickUp", true);
        interact = false;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        FirstPersonController fpc = other.GetComponentInChildren<FirstPersonController>();
        if (fpc && interact)
        {
            fpc.Interact += Interact;
            UiManager.instance.EnableInteractMessage(true);
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        FirstPersonController fpc = other.GetComponentInChildren<FirstPersonController>();
        if (fpc)
        {
            fpc.Interact -= Interact;
            UiManager.instance.EnableInteractMessage(false);
            UiManager.instance.SetInfoMessage("");
        }
    }
}
