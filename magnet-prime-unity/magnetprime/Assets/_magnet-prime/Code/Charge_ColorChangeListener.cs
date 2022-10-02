using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Charge_ColorChangeListener : MonoBehaviour
{
    public Charge chargeToListen;
    public string matColorAddress;
    public Material myMat;
    public Light myLight;
    public bool enableEmission;
    [SerializeField] bool pulse = false;
    
    void Start()
    {
        chargeToListen.polarityChange += ColorChangeListener;
        var renderer = GetComponent<Renderer>();
        myMat = renderer.material;
        if(enableEmission) { myMat.EnableKeyword("_EMISSION"); }
        myMat.EnableKeyword(matColorAddress);
        PulseColor();
    }

    void ColorChangeListener(int polarity)
    {
        if (polarity > 0)
        {
            myMat.SetColor(matColorAddress, Color.red);
            PulseColor();
            if (myLight) { myLight.color = Color.red; }
            return;
        }
        else if (polarity < 0)
        {
            myMat.SetColor(matColorAddress, Color.blue);
            PulseColor();
            if (myLight) { myLight.color = Color.blue; }
            return;
        }
        myMat.SetColor(matColorAddress, Color.white);
        PulseColor();
        if (myLight) { myLight.color = Color.white; }
        return;
    }

    void PulseColor()
    {
        if (pulse)
        {
            myMat.DOKill();
            myMat.DOColor(new Color(myMat.color.r * 0.75f, myMat.color.g * 0.75f, myMat.color.b * 0.75f), matColorAddress, 1f).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
