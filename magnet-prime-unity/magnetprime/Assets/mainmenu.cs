using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public Animator bgAnim;
    public void PlayGame()
    {
        bgAnim.SetTrigger("FadeOut");
        Invoke("LoadGame", 1f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
