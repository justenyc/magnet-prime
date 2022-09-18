using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Interactable_KeyDoor : Interactable
{
    public List<Interactable_Collectable> KeysRequired;
    Dictionary<Interactable_Collectable, bool> keysFound;
    public LayerMask Mask;
    public Animator animator;

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
                //GetComponent<MeshRenderer>().enabled = false;
                animator.SetTrigger("Open");
                OnDisable();
                foreach (Collider c in GetComponentsInChildren<Collider>())
                    c.enabled = false;
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "Door_Open");
                UiManager.instance.SetInfoMessage("");
            }
            else
            {
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyNotFound");
                UiManager.instance.SetInfoMessage("Required Keys Not Found");
            }
        }
        else
        {
            SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyNotFound");
            UiManager.instance.SetInfoMessage("Required Keys Not Found");
        }
    }

    private void InitializeKeyDict()
    {
        keysFound = new Dictionary<Interactable_Collectable, bool>();
        foreach (Interactable_Collectable key in KeysRequired)
        {
            if(key == null)
            {
                Debug.LogError($"KeysRequired from {this.name} cannot contain a null value!");
                return;
            }
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

    private void OnDisable()
    {
        Collider col = GetComponent<Collider>();
        Collider[] cols;
        if (col.GetType() == typeof(BoxCollider))
        {
            BoxCollider box = col.GetComponent<BoxCollider>();

            cols = Physics.OverlapBox(transform.position + (box.center), box.size);
            //Debug.Log($"SearchCenter: {transform.position + (box.center * -1)}, BoxCenter: {box.center}, \nSize: {box.size}");

            foreach (Collider c in cols)
            {
                Debug.Log(c);
                OnTriggerExit(c);
            }
        }
    }
}
