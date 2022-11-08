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
    [SerializeField] FirstPersonController fpc;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] Image overlay;
    [SerializeField] List<string> introMessages;

    private void Start()
    {
        Intro();
    }

    public void Intro()
    {
        fpc.EnableController(false);
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Area 1"))
        {
            StartCoroutine(IntroRoutine());
        }
    }

    IEnumerator IntroRoutine()
    {
        foreach(string s in introMessages)
        {
            yield return FillText(s);
            yield return new WaitForSeconds(2f);
        }
        overlay.DOFade(0, 1f);
        mainText.DOFade(0, 1f).onComplete += () => this.gameObject.SetActive(false);
        fpc.EnableController(true);
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
