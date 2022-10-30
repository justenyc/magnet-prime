using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using DG.Tweening;

public class Interactable_KeyDoor : Interactable
{
    public List<Interactable_Collectable> KeysRequired;
    Dictionary<Interactable_Collectable, bool> keysFound;
    public int numKeysRequired = 1;
    public LayerMask Mask;

    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        InitializeKeyDict();
    }

    private void Update()
    {
        
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
                transform.DOMoveZ(transform.position.z + 3, 1);
                OnDisable();
                foreach (Collider c in GetComponentsInChildren<Collider>())
                    c.enabled = false;
                foreach (Interactable_Collectable key in KeysRequired)
                    Inventory.instance.items.Remove(key);
                SfxManager.instance.PlayFromSource(audioSource, "Door_Open");
                UiManager.instance.SetInfoMessage("");
            }
            else
            {
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyNotFound");
                UiManager.instance.SetInfoMessage($"{numKeysRequired} keys required");
            }
        }
        else
        {
            SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "KeyNotFound");
            UiManager.instance.SetInfoMessage($"{numKeysRequired} keys required");
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

    public override void OnTriggerEnter(Collider other)
    {
        base.customEvent.AddListener(Interact);
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        base.customEvent.RemoveListener(Interact);
        base.OnTriggerExit(other);
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
                //Debug.Log(c);
                OnTriggerExit(c);
            }
        }
    }
}
