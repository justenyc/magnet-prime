using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    Dictionary<string, AudioClip> acDict;

    private void Start()
    {
        InitializeDict();
    }

    void InitializeDict()
    {
        foreach(AudioClip ac in audioClips)
        {
            acDict.Add(ac.name, ac);
        }
    }

    public AudioClip GetClipByName(string name)
    {
        return acDict[name] ?? null;
    }
}
