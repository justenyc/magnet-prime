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
    [SerializeField] AudioListener audioListener;
    [SerializeField] List<AudioSource> currentlyPlaying = new List<AudioSource>();

    Dictionary<string, AudioClip> acDict;

    private void Awake()
    {
        acDict = new Dictionary<string, AudioClip>();
        instance = instance ? instance : this;
        mainSource = mainSource ? mainSource : GameObject.FindObjectOfType<FirstPersonController>().audioSource;
        InitializeDict();
    }

    private void Start()
    {
        audioListener = audioListener ?? Camera.main?.GetComponent<AudioListener>();
    }

    void InitializeDict()
    {
        foreach (AudioClip ac in audioClips)
        {
            acDict.Add(ac.name, ac);
        }
    }

    public AudioClip GetClipByName(string name)
    {
        AudioClip clip = acDict[name] ?? null;
        if(clip == null) { Debug.LogError($"No clip found with name: {name}"); }
        return clip;
    }

    #region Public Actions
    public void PlayFromSource(AudioSource aSource, string sfxName, bool oneshot = false, bool loop = false, bool play = true)
    {
        if (play && currentlyPlaying.Count < 17)
        {
            aSource.loop = loop;
            aSource.clip = GetClipByName(sfxName);
            AddCurrentlyPlaying(aSource);

            if (oneshot)
            {

                aSource.PlayOneShot(aSource.clip);
            }
            else
            {
                aSource.Play();
            }
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

    void AddCurrentlyPlaying(AudioSource newSource)
    {
        currentlyPlaying.Add(newSource);
        StartCoroutine(DelayAction(() => currentlyPlaying.Remove(newSource), newSource.clip.length));
        return;
    }

    IEnumerator DelayAction(Action action, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        action();
    }

    IEnumerator DelayAction<T>(Action<T[]> action, float delayTime, T[] args = null)
    {
        yield return new WaitForSeconds(delayTime);
        action(args);
    }
}
