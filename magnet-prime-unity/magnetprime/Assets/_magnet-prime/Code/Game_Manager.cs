using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{ 
    public static Game_Manager instance;
    public VfxManager vfxManager;
    public string state;

    public bool paused { get; set; } = false;
    public HashSet<Interactable_Collectable> loreLogs = new HashSet<Interactable_Collectable>();

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {

    }

    public void PauseGame(bool pause)
    {
        paused = pause;
        if (paused == true)
        {
            FindObjectOfType<FirstPersonController>().enabled = false;
            Time.timeScale = 0f;
            UiManager.instance.GetCrosshair().enabled = false;
            Cursor.visible = true;
        }
        else if (paused == false)
        {
            FindObjectOfType<FirstPersonController>().enabled = true;
            Time.timeScale = 1;
            UiManager.instance.GetCrosshair().enabled = true;
            Cursor.visible = false;
        }
    }

    public void ResumeGame()
    {
        FindObjectOfType<FirstPersonController>().enabled = true;
        Time.timeScale = 1;
        UiManager.instance.GetCrosshair().enabled = true;
        Cursor.visible = false;
        paused = false;
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ShowPauseScreen(bool show)
    {
        UiManager.instance.ShowPauseScreen(show);
    }


    public void SetState(string s)
    {
        state = s;
    }
}