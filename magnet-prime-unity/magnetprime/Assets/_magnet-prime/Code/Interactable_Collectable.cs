using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
                Game_Manager.instance.loreLogs.Add(this);
                if (Game_Manager.instance.paused)
                {
                    Game_Manager.instance.state = "";
                    UiManager.instance.CloseTextBox();
                    UiManager.instance.EnableInteractMessage(true);
                    SfxManager.instance.SetVolume(SfxManager.instance.mainSource, 0.5f);
                    SfxManager.instance.SetPitch(SfxManager.instance.mainSource, 0.5f);
                    SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "TextBoxOpen", oneshot: true);
                    Game_Manager.instance.PauseGame(false);
                }
                else
                {
                    Game_Manager.instance.state = "LoreLog";
                    UiManager.instance.UpdateTextBoxText(message);
                    UiManager.instance.OpenTextBox(message);
                    UiManager.instance.EnableInteractMessage(false);
                    SfxManager.instance.SetVolume(SfxManager.instance.mainSource, 0.5f);
                    SfxManager.instance.SetPitch(SfxManager.instance.mainSource, 1);
                    SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "TextBoxOpen", oneshot: true);
                    Game_Manager.instance.PauseGame(true);
                }
                return;

            case Type.Key:
                Inventory.instance.AddItem(this);
                UiManager.instance.EnableInteractMessage(false);
                fpc.Interact -= Interact;
                SfxManager.instance.SetVolume(SfxManager.instance.mainSource, 1);
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyPickUp", oneshot: true);
                GetComponentInChildren<MeshRenderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
                GetComponent<Rotator>().enabled = false;
                //Destroy(this.gameObject, 1f);
                return;

            case Type.Core:
                CutSceneManager.instance.PlayEndingCutscene();
                return;

            default:
                return;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        fpc = other.GetComponentInChildren<FirstPersonController>();
        if (fpc && fpc.Interact == null)
        {
            fpc.Interact += Interact;
            UiManager.instance.EnableInteractMessage(true);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (fpc)
        {
            fpc.Interact -= Interact;
            UiManager.instance.EnableInteractMessage(false);
        }
        fpc = null;
    }

    private void OnDestroy()
    {
        try
        {
            fpc.Interact -= Interact;
        }
        catch
        {

        }
    }
}

public enum Type
{
    Log,
    Key,
    Core
};
