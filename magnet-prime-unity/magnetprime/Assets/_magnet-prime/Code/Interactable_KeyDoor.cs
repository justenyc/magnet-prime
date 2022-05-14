using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Interactable_KeyDoor : Interactable
{
    public List<Interactable_Collectable> KeysRequired;
    Dictionary<Interactable_Collectable, bool> keysFound;
    public MeshRenderer myMesh;
    public Collider myCollider;

    private void Start()
    {
        InitializeKeyDict();
    }

    public override void Interact()
    {
        if (Inventory.instance.items.Count > 0)
        {
            foreach (Interactable_Collectable key in KeysRequired)
            {
                if (Inventory.instance.SearchForItem(key))
                {
                    keysFound[key] = true;
                }
            }

            if (KeysCheck())
            {
                myMesh.enabled = false;
                myCollider.enabled = false;
                UiManager.instance.SetInfoMessage("");
            }
            else
            {
                UiManager.instance.SetInfoMessage("Required Keys Not Found");
            }
        }
        else
        {
            UiManager.instance.SetInfoMessage("Required Keys Not Found");
        }
    }

    private void InitializeKeyDict()
    {
        keysFound = new Dictionary<Interactable_Collectable, bool>();
        foreach (Interactable_Collectable key in KeysRequired)
        {
            keysFound.Add(key, false);
        }
    }

    void DebugDict()
    {
        foreach (Interactable_Collectable key in KeysRequired)
        {
            Debug.Log(keysFound[key] + " " + key);
        }
    }

    bool KeysCheck()
    {
        foreach (Interactable_Collectable key in KeysRequired)
        {
            if (!keysFound[key])
            {
                return false;
            }
        }
        return true;
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
            UiManager.instance.SetInfoMessage("");
        }
    }
}
