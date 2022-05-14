using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public GameObject interactMessage;
    public TextMeshProUGUI infoMessage;
    public static UiManager instance;

    private void Start()
    {
        instance = this;
    }

    public void EnableInteractMessage(bool enable)
    {
        interactMessage.SetActive(enable);
    }

    public void SetInfoMessage(string newMessage)
    {
        infoMessage.text = newMessage;
    }
}
