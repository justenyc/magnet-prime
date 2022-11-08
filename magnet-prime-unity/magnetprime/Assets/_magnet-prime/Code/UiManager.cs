using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.EventSystems;
using System.Linq;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [Header("HUD Elements")]
    public Image fullScreenImage;
    public TextMeshProUGUI infoMessage;
    public TextMeshProUGUI textBoxText;
    public GameObject interactMessage;
    public RectTransform textBox;
    public RectTransform crosshair;
    public Image hand;
    public float fadeTextTime = 2f;

    [Header("Pause Canvas Elements")]
    public Transform pauseCanvas;
    public TextMeshProUGUI logText;
    public RectTransform keyContainer;
    public List<Image> keyImages = new List<Image>();
    public EventSystem eventSystem;
    public InputSystemUIInputModule inputUiModule;
    public GameObject resumeButton;

    [Header("References")]
    public PlayerInput playerInput;

    private void Start()
    {
        instance = this;
        if (inputUiModule == null)
        {
            inputUiModule = FindObjectOfType<InputSystemUIInputModule>();
        }

        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        keyImages = keyContainer.GetComponentsInChildren<Image>().ToList();
        FadeFullScreenImage(false);
    }

    public void FadeFullScreenImage(bool colorIn, float fadeDuration = 0.5f, TweenCallback callback = null)
    {
        Tween tween = fullScreenImage.DOFade(colorIn ? 1 : 0, fadeDuration);
        tween.OnComplete(callback);
    }

    public void FadeFullScreenImage(bool colorIn, Color newColor, float fadeDuration = 0.5f, TweenCallback callback = null)
    {
        fullScreenImage.color = newColor;
        FadeFullScreenImage(colorIn, fadeDuration, callback);
    }

    public void EnableInteractMessage(bool enable)
    {
        string interactMesssageText = playerInput.currentControlScheme == "Gamepad" ?
                    "Press Triangle to interact" :
                    "Press E to interact";

        interactMessage.GetComponent<TextMeshProUGUI>().text = interactMesssageText;
        interactMessage.SetActive(enable);
    }

    public void SetInfoMessage(string newMessage)
    {
        int tweens = infoMessage.DOKill();
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

    public void TutorialMessageTable(string key)
    {
        switch(key)
        {
            case ("tut1"):
                SetInfoMessageText(playerInput.currentControlScheme == "Gamepad" ? 
                    "Press Square to pick up or drop objects" : 
                    "Press F to pick up or drop objects");
                break;

            case ("tut2"):
                SetInfoMessageText(playerInput.currentControlScheme == "Gamepad" ?
                    "L1 or R1 to apply positive or negative charges to metallic objects" : 
                    "Left or right click to apply positive or negative charges to metallic objects");
                break;

            case ("tut3"):
                SetInfoMessageText("Energy cells create magnetic fields that will push or pull objects based on their polarity");
                break;

            case ("tut4"):
                SetInfoMessageText(playerInput.currentControlScheme == "Gamepad" ?
                    "L2 changes your own polarity, R2 will push or pull yourself or objects based off of your current polarity" : 
                    "CTRL changes your own polarity, Shift will push or pull yourself or objects based off of your current polarity");
                break;

            case ("tut5"):
                SetInfoMessageText(playerInput.currentControlScheme == "Gamepad" ?
                    "Press Options to pause and check your current inventory" :
                    "Press Tab to pause and check your current inventory");
                break;
        }
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
        inputUiModule.enabled = show;
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

    public void UpdatePauseDisplayData()
    {
        instance.logText.text = Game_Manager.instance.loreLogs.Count.ToString();
        int keyCount = Inventory.instance.items.Count;

        for (int ii = 0; ii < keyImages.Count; ii++)
        {
            if(ii < keyCount)
            {
                keyImages[ii].enabled = true;
            }
            else
            {
                keyImages[ii].enabled = false;
            }
        }
    }

    public void ShowHand(bool show)
    {
        hand.color = new Color(hand.color.r, hand.color.g, hand.color.b, show ? 1 : 0);
    }

    public void ChangeCrosshairColor(Color newColor)
    {
        crosshair.GetComponent<Image>().color = newColor;
    }
}
