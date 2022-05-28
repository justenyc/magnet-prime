using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class VisorEmissionController : MonoBehaviour
{
    public Color positiveColor = Color.red;
    public Color negativeColor = Color.blue;
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        FirstPersonController fpc = FindObjectOfType<FirstPersonController>();
        fpc.PolarityChanged += ChangeColor;
        mat = gameObject.GetComponent<MeshRenderer>().material;
        mat.EnableKeyword("_EmissionColor");
    }

    void ChangeColor(int polarity)
    {
        if(polarity == 1)
        {
            mat.SetColor("_EmissionColor", positiveColor);
            return;
        }
        mat.SetColor("_EmissionColor", negativeColor);
    }
}
