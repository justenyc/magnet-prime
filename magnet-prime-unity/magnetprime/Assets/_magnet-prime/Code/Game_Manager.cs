using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Game_Manager : MonoBehaviour
{ 
    public static Game_Manager instance;
    public VfxManager vfxManager;
    public string state;

    public bool paused = false;
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
    }

    public void PauseGame(bool pause)
    {
        paused = pause;
        if (paused == true)
        {
            FindObjectOfType<FirstPersonController>().enabled = false;
            Time.timeScale = 0f;
            UiManager.instance.GetCrosshair().enabled = false;
        }
        else if (paused == false)
        {
            FindObjectOfType<FirstPersonController>().enabled = true;
            Time.timeScale = 1;
            UiManager.instance.GetCrosshair().enabled = true;
        }
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