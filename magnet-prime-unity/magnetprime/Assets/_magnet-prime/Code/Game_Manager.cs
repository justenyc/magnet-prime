using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Game_Manager : MonoBehaviour
{ 
    public static Game_Manager instance;
    public string state;

    public bool paused = false;

    private void Start()
    {
        instance = instance ?? this;
    }

    private void Update()
    {
        if (paused == true)
            Time.timeScale = 0f;
        else if (paused == false)
            Time.timeScale = 1;
    }

    public void PauseGame(bool pause)
    {
        Debug.Log($"PauseGame: {pause}");
        paused = pause;
    }

    public void SetState(string s)
    {
        state = s;
    }
}