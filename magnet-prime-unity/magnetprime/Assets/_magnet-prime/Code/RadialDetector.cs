using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialDetector : MonoBehaviour
{
    public Magnetism_Immovable root;
    public Color positiveColor;
    public Color negativeColor;
    public Color neutralColor;
    public Material mat;

    private void Start()
    {
        mat = this.GetComponent<MeshRenderer>().material;
        root.polarityChange += PolarityChangeListener;
    }

    private void OnTriggerEnter(Collider other)
    {
        Magnetism_Movable temp = other.GetComponent<Magnetism_Movable>();
        mat = this.GetComponent<MeshRenderer>().material;
        if (temp != null)
            root.AddMovable(temp);
    }

    private void OnTriggerExit(Collider other)
    {
        Magnetism_Movable temp = other.GetComponent<Magnetism_Movable>();
        if (temp != null)
        {
            try
            {
                temp.GetComponent<Rigidbody>().useGravity = true;
                temp.beingMagnetized = false;
                root.movableObjectsWithCharge.Remove(temp);
                root.RemoveLine();
            }
            catch
            {

            }
        }
    }

    void PolarityChangeListener(int ii)
    {
        //Debug.Log("RadialDetector.RadialChangeListener Called");
        if (ii > 0)
        {
            mat.SetColor("Color_Main", positiveColor);
        }
        else if (ii < 0)
        {
            mat.SetColor("Color_Main", negativeColor);
        }
        else
        {
            mat.SetColor("Color_Main", neutralColor);
        }
    }
}
