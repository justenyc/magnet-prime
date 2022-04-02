using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDependent_Door : SwitchDependent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    public override void OnSwitch(bool active)
    {
        Debug.Log(this.name + " | OnSwitch()");
        this.GetComponent<MeshRenderer>().enabled = !active;
    }
}
