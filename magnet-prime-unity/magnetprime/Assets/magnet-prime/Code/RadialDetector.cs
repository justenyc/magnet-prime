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
            root.movableObjectsWithCharge.Add(temp);
    }

    private void OnTriggerExit(Collider other)
    {
        Magnetism_Movable temp = other.GetComponent<Magnetism_Movable>();
        if (temp != null)
            root.movableObjectsWithCharge.Remove(temp);
    }
}
