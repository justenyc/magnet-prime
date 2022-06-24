using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

public class Interactable_Collectable : Interactable
{
    public Type type;
    public string message = "<ERROR_CODE_404> MESSAGE NOT FOUND";

    FirstPersonController fpc;
    public override void Interact()
    {
        //Input System Package > Update Mode must be "Process Events In Dynamic Update"
        switch (type)
        {
            case Type.Log:
                Game_Manager.instance.PauseGame(Game_Manager.instance.paused ? false : true);
                if (Game_Manager.instance.paused)
                    UiManager.instance.OpenTextBox(message);
                else
                    UiManager.instance.CloseTextBox();
                return;

            case Type.Key:
                Inventory.instance.AddItem(this);
                UiManager.instance.EnableInteractMessage(false);
                fpc.Interact -= Interact;
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyPickUp", true);
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                Destroy(this.gameObject, 1f);
                return;

            default:
                return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        fpc = other.GetComponentInChildren<FirstPersonController>();
        if (fpc)
        {
            fpc.Interact += Interact;
            UiManager.instance.EnableInteractMessage(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (fpc)
        {
            fpc.Interact -= Interact;
            UiManager.instance.EnableInteractMessage(false);
        }
        fpc = null;
    }
}

public enum Type
{
    Log,
    Key
};
