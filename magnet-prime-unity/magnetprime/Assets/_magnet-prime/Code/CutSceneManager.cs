using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using StarterAssets;
using DG.Tweening;
using TMPro;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;
    [SerializeField] FirstPersonController fpc;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] Image overlay;
    [SerializeField] List<string> introMessages;
    [SerializeField] List<string> endingMessages;
    [SerializeField] bool playIntro = true;

    private void Start()
    {
        instance = instance ?? this;
        if (playIntro) { 
            PlayIntroCutscene(); 
        }
    }

    public void PlayIntroCutscene()
    {
        fpc.EnableController(false);
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Area 1") || playIntro)
        {
            StartCoroutine(ScrollMessagesRoutine(introMessages, () =>
            {
                overlay.DOFade(0, 1f);
                mainText.DOFade(0, 1f);
                fpc.EnableController(true);
            }));
        }
    }

    public void PlayEndingCutscene()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Area 4"))
        {
            StartCoroutine(ScrollMessagesRoutine(endingMessages, 
            () =>
            {
                mainText.DOFade(0, 2f).onComplete += () => { SceneManager.LoadScene("Title"); };
            },
            () =>
            {
                overlay.DOFade(1, 0.5f);
                mainText.DOFade(1, 0.5f);
                fpc.EnableController(false);
            }));
        }
    }

    IEnumerator ScrollMessagesRoutine(List<string> messages, System.Action onComplete = null, System.Action onStart = null)
    {
        if (onStart != null) { onStart(); }
        foreach(string s in messages)
        {
            yield return FillText(s);
            yield return new WaitForSeconds(2f);
        }
        if (onComplete != null) { onComplete(); }
    }

    IEnumerator FillText(string s)
    {
        var message = s;
        var temp = "";
        for (int ii = 0; ii < message.Length; ii++)
        {
            temp = temp + message[ii];
            mainText.text = temp;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
