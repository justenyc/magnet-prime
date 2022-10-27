using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererHelper : MonoBehaviour
{
    public LineRenderer lr;
    public Vector3[] positions;
    public Transform target;
    public float noiseStrength = 0.1f;
    public float numberOfDivisions = 4;
    public float colorAlpha = 1;

    public void EnableRenderer(bool enable)
    {
        lr.enabled = enable;
    }

    public void DrawLine(Vector3 endPosition, float numberOfDivisions, System.Action modifier)
    {
        endPosition = Camera.main.transform.InverseTransformPoint(endPosition);

        positions = new Vector3[(int)numberOfDivisions + 1];

        for (int ii = 0; ii < positions.Length; ii++)
        {
            positions[ii] = endPosition * (ii / numberOfDivisions);
        }        
        positions[positions.Length - 1] = endPosition;
        lr.positionCount = positions.Length;
        
        modifier();
        lr.SetPositions(positions);
    }

    public void AddNoiseToPositions()
    {
        for (int ii = 1; ii < positions.Length - 1; ii++)
        {
            positions[ii] = new Vector3(positions[ii].x + Random.Range(-noiseStrength, noiseStrength), positions[ii].y + Random.Range(-noiseStrength, noiseStrength), positions[ii].z);
        }
    }

    public void ChangeColor(Color newColor)
    {
        lr.startColor = new Color(newColor.r, newColor.g, newColor.b, colorAlpha);
        lr.endColor = new Color(newColor.r, newColor.g, newColor.b, colorAlpha);
    }
}
