using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDependent : MonoBehaviour
{
    public Switch[] switches;
    // Start is called before the first frame update
    public void Start()
    {
        if (switches.Length > 0)
        {
            foreach (Switch s in switches)
            {
                s.SwitchAction += SwitchListener;
            }
        }
    }

    void SwitchListener()
    {
        if (switches.Length > 0)
        {
            for (int ii = 0; ii < switches.Length; ii++)
            {
                if (switches[ii].switchActive == false)
                {
                    OnSwitch(false);
                    return;
                }
            }
            OnSwitch(true);
        }
    }

    public virtual void OnSwitch(bool active)
    {

    }
}
