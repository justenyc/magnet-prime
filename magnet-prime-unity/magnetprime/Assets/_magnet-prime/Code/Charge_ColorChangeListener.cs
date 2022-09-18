using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_ColorChangeListener : MonoBehaviour
{
    public Charge chargeToListen;
    public string matColorAddress;
    public Material myMat;
    public Light myLight;
    
    void Start()
    {
        chargeToListen.polarityChange += ColorChangeListener;
        var renderer = GetComponent<Renderer>();
        myMat = renderer.material;
    }

    void ColorChangeListener(int polarity)
    {
        if (polarity > 0)
        {
            myMat.SetColor(matColorAddress, Color.red);
            if (myLight) { myLight.color = Color.red; }
            return;
        }
        else if (polarity < 0)
        {
            myMat.SetColor(matColorAddress, Color.blue);
            if (myLight) { myLight.color = Color.blue; }
            return;
        }
        myMat.SetColor(matColorAddress, Color.white);
        if (myLight) { myLight.color = Color.white; }
        return;
    }
}
