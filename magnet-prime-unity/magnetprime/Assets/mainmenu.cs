using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public Animator bgAnim;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        bgAnim.SetTrigger("FadeOut");
        Invoke("LoadGame", 1f);
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
