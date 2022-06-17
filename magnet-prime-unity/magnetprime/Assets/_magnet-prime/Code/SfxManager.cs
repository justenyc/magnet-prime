using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    Dictionary<string, AudioClip> acDict;
    public static SfxManager instance;

    private void Start()
    {
        acDict = new Dictionary<string, AudioClip>();
        instance = instance ? instance : this;
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
        return acDict[name] ? acDict[name] : null;
    }

    public void PlayFromSource(AudioSource aSource, string sfxName)
    {
        aSource.Stop();
        aSource.clip = GetClipByName(sfxName);
        aSource.Play();
    }

    public void RandomizePitch(AudioSource aSource, float min = 0.1f, float max = 2f)
    {
        aSource.pitch = UnityEngine.Random.Range(min, max);
    }

    public void SetVolume(AudioSource aSource, float newVolume)
    {
        aSource.volume = newVolume;
    }
}
