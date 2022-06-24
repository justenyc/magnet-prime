using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using StarterAssets;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;
    public AudioClip[] audioClips;
    public AudioSource mainSource;
    Dictionary<string, AudioClip> acDict;

    private void Start()
    {
        acDict = new Dictionary<string, AudioClip>();
        instance = instance ? instance : this;
        mainSource = mainSource ? mainSource : GameObject.FindObjectOfType<FirstPersonController>().audioSource;
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

    public void PlayFromSource(AudioSource aSource, string sfxName, bool oneshot = false, bool loop = false, bool play = true)
    {
        if (play)
        {
            aSource.loop = loop;
            aSource.Stop();
            aSource.clip = GetClipByName(sfxName);
            if (oneshot)
                aSource.PlayOneShot(aSource.clip);
            else
                aSource.Play();
        }
        else
        {
            aSource.Stop();
        }
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
