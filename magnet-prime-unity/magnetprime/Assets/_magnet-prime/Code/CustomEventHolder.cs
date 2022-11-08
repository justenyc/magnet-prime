using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using StarterAssets;

public class CustomEventHolder : MonoBehaviour
{
    public UnityEvent onExitEvent;
    public UnityEvent eventToCall;
    public Action customAction;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FirstPersonController>(out FirstPersonController fpc))
        {
            eventToCall.Invoke();
            //Debug.Log("Yes this is player");
        }
    }
    public void A_LoadSceneAdditive(string s)
    {
        var newSceneOperation = SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
    }

    public void A_EnableObject(GameObject go)
    {
        go.SetActive(true);
    }

    public void A_DisplayMessage(string message)
    {
        UiManager.instance.TutorialMessageTable(message);
        UiManager.instance.FadeInText();
    }

    public void A_DisplayMessageFadeOut()
    {
        UiManager.instance.FadeOutText();
    }

    public void A_DisableBox()
    {
        this.GetComponent<Collider>().enabled = false;
        //this.enabled = false;
    }

    public void A_ChangeActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<FirstPersonController>(out FirstPersonController fpc))
        {
            onExitEvent.Invoke();
            //Debug.Log("Yes this is player");
        }
    }

    public void PostCustomAction()
    {
        if (customAction != null)
        {
            customAction();
        }
    }
}
