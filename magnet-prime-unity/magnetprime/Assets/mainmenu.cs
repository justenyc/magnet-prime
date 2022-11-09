using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class mainmenu : MonoBehaviour
{
    public Image overlay;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        overlay.DOKill();
        overlay.DOFade(1, 1f).onComplete += () =>
        {
            LoadGame();
        };
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Quitting Game");
#endif
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
