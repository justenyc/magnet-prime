using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CustomEventHolder : MonoBehaviour
{
    public static CustomEventHolder instance;
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
        eventToCall.Invoke();
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
        UiManager.instance.SetInfoMessage(message);
    }
}
