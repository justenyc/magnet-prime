using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialDetector : MonoBehaviour
{
    public Magnetism_Immovable root;
    public Color positiveColor;
    public Color negativeColor;
    public Material mat;
    private void OnTriggerEnter(Collider other)
    {
        Magnetism_Movable temp = other.GetComponent<Magnetism_Movable>();
        mat = this.GetComponent<MeshRenderer>().material;
        if (temp != null)
            root.AddMovable(temp);
        root.polarityChange += PolarityChangeListener;
    }

    private void OnTriggerExit(Collider other)
    {
        Magnetism_Movable temp = other.GetComponent<Magnetism_Movable>();
        if (temp != null)
        {
            try
            {
                root.movableObjectsWithCharge.Remove(temp);
            }
            catch
            {

            }
        }
    }

    void PolarityChangeListener(int ii)
    {
        Debug.Log("RadialDetector.RadialChangeListener Called");
        if (ii > 0)
        {
            mat.SetColor("Color_Main", positiveColor);
            mat.SetFloat("Alpha", 0);
        }
        else if (ii < 0)
        {
            mat.SetColor("Color_Main", negativeColor);
            mat.SetFloat("Alpha", 0);
        }
        else
        {
            mat.SetFloat("Alpha", 1);
        }
    }
}
