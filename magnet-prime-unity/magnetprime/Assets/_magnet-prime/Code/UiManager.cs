using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [Header("HUD Elements")]
    public TextMeshProUGUI infoMessage;
    public TextMeshProUGUI textBoxText;
    public GameObject interactMessage;
    public RectTransform textBox;
    public RectTransform crosshair;
    public float fadeTextTime = 2f;

    [Header("Pause Canvas Elements")]
    public Transform pauseCanvas;
    public TextMeshProUGUI logText;
    public EventSystem eventSystem;
    public GameObject resumeButton;

    private void Start()
    {
        instance = this;
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }
    }

    public void EnableInteractMessage(bool enable)
    {
        interactMessage.SetActive(enable);
    }

    public void SetInfoMessage(string newMessage)
    {
        int tweens = infoMessage.DOKill();
        //Debug.Log($"Killed {tweens} tweens");
        infoMessage.DOFade(1, 0.25f).onComplete = () =>
        {
            infoMessage.DOFade(0, 1f).SetDelay(5f);
        };
        infoMessage.text = newMessage;
    }

    public void SetInfoMessageText(string newMessage)
    {
        infoMessage.text = newMessage;
    }

    public void FadeInText()
    {
        infoMessage.DOKill();
        infoMessage.DOFade(1, 0.25f);
    }

    public void FadeOutText()
    {
        infoMessage.DOKill();
        infoMessage.DOFade(0, fadeTextTime);
    }

    public void CloseTextBox()
    {
        textBox.DOScaleX(0, 0.5f).SetUpdate(true);
    }

    public void OpenTextBox(string message)
    {
        textBox.DOScaleX(1, 0.5f).SetUpdate(true);
    }

    public void UpdateTextBoxText(string newMessage)
    {
        textBoxText.text = newMessage;
    }

    public void PauseAnimation(string animationName, bool show)
    {
        eventSystem.enabled = show;
        pauseCanvas.GetComponent<Animator>().Play(animationName);
    }

    public Image GetCrosshair()
    {
        return crosshair.GetComponent<Image>();
    }

    public void ShowPauseScreen(bool show)
    {
        if (show)
        {
            UiManager.instance.PauseAnimation("Pause_anim", show);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            UiManager.instance.PauseAnimation("Unpause_anim", show);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SetSelectedObject(GameObject newObject)
    {
        eventSystem.SetSelectedGameObject(resumeButton);
    }
}
