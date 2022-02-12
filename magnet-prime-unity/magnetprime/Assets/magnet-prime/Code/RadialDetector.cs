using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialDetector : MonoBehaviour
{
    public Magnetism_Immovable root;
    private void OnTriggerEnter(Collider other)
    {
        Magnetism_Movable temp = other.GetComponent<Magnetism_Movable>();
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
                root.movableObjectsWithCharge.Remove(temp);
            }
            catch
            {

            }
        }
    }
}
