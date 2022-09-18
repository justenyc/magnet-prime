using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public GameObject interactMessage;
    public RectTransform textBox;

    public TextMeshProUGUI infoMessage;
    public TextMeshProUGUI textBoxText;

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
        int tweens = infoMessage.DOKill();
        Debug.Log($"Killed {tweens} tweens");
        infoMessage.DOFade(1, 0.25f).onComplete = () =>
        {
            infoMessage.DOFade(0, 1f).SetDelay(10f);
        };
        infoMessage.text = newMessage;
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
}
