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
    [SerializeField] float playCd = 0.5f;
    [SerializeField] float playCdTimer;

    Dictionary<string, AudioClip> acDict;

    private void Awake()
    {
        acDict = new Dictionary<string, AudioClip>();
        instance = instance ? instance : this;
        mainSource = mainSource ? mainSource : GameObject.FindObjectOfType<FirstPersonController>().audioSource;
        InitializeDict();
    }

    void InitializeDict()
    {
        foreach (AudioClip ac in audioClips)
        {
            acDict.Add(ac.name, ac);
        }
    }

    /*private void FixedUpdate()
    {
        if (playCdTimer > 0)
            playCdTimer -= Time.deltaTime;
    }*/

    public AudioClip GetClipByName(string name)
    {
        return acDict[name] ? acDict[name] : null;
    }

    #region Public Actions
    public void PlayFromSource(AudioSource aSource, string sfxName, bool oneshot = false, bool loop = false, bool play = true)
    {
        if (play)
        {
            aSource.loop = loop;
            aSource.clip = GetClipByName(sfxName);

            if (oneshot)
            {
                aSource.PlayOneShot(aSource.clip);
            }
            else
            {
                aSource.Play();
            }

            playCdTimer = playCd;
        }
        else
        {
            aSource.Stop();
        }
    }

    public void ResetSourceSettings(AudioSource aSource)
    {
        aSource.pitch = 1;
        aSource.volume = 1;
    }

    public void RandomizePitch(AudioSource aSource, float min = 0.1f, float max = 2f)
    {
        aSource.pitch = UnityEngine.Random.Range(min, max);
    }

    public void SetVolume(AudioSource aSource, float newVolume)
    {
        aSource.volume = newVolume;
    }

    public void SetPitch(AudioSource aSource, float pitch)
    {
        aSource.pitch = pitch;
    }
    #endregion

    IEnumerator DelayFunction<T>(float delayTime, System.Action<T> callback, T arg)
    {
        yield return new WaitForSeconds(delayTime);
        callback(arg);
    }
}
