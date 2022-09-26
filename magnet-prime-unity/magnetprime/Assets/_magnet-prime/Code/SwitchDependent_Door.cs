using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchDependent_Door : SwitchDependent
{
    [SerializeField] float startingZ;
    // Start is called before the first frame update
    void Start()
    {
        startingZ = transform.position.z;
        base.Start();
    }

    public override void OnSwitch(bool active)
    {
        //Debug.Log(this.name + " | OnSwitch()");
        //this.gameObject.SetActive(!active);
        if (active)
        {
            transform.DOMoveZ(startingZ + 3, 1f);
        }
        else
        {
            transform.DOMoveZ(startingZ, 1f);
        }
    }
}
