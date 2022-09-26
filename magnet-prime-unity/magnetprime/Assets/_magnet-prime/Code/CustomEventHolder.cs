using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using StarterAssets;

public class CustomEventHolder : MonoBehaviour
{
    public static CustomEventHolder instance;
    public UnityEvent onExitEvent;
    public UnityEvent eventToCall;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FirstPersonController>(out FirstPersonController fpc))
        {
            eventToCall.Invoke();
            Debug.Log("Yes this is player");
        }
    }
    public void A_LoadSceneAdditive(string s)
    {
        SceneManager.LoadScene(s, LoadSceneMode.Additive);
    }

    public void A_EnableObject(GameObject go)
    {
        go.SetActive(true);
    }

    public void A_DisplayMessage(string message)
    {
        UiManager.instance.SetInfoMessageText(message);
        UiManager.instance.FadeInText();
    }

    public void A_DisplayMessageFadeOut()
    {
        UiManager.instance.FadeOutText();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<FirstPersonController>(out FirstPersonController fpc))
        {
            onExitEvent.Invoke();
            Debug.Log("Yes this is player");
        }
    }
}
