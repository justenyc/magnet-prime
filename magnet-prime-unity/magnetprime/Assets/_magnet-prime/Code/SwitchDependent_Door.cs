using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwitchDependent_Door : SwitchDependent
{
    [SerializeField] float startingZ;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool doorOpen;
    // Start is called before the first frame update
    void Start()
    {
        startingZ = transform.position.z;
        base.Start();
    }

    public override void OnSwitch(bool active)
    {
        if (active)
        {
            transform.DOMoveZ(startingZ + 3, 1f);
            if (!audioSource.isPlaying && !doorOpen)
            {
                SfxManager.instance.PlayFromSource(audioSource, "Door_Open", true);
            }
            doorOpen = active;
        }
        else
        {
            transform.DOMoveZ(startingZ, 1f);
            doorOpen = active;
        }
    }
}
