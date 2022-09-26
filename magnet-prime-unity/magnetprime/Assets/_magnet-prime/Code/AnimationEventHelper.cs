using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    public GameObject resumeButton;

    public void SetResume()
    {
        UiManager.instance.SetSelectedObject(resumeButton);
    }   
}
