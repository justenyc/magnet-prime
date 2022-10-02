using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererHelper : MonoBehaviour
{
    public LineRenderer lr;
    public Vector3[] positions;
    public Transform target;
    public float noiseStrength = 1;
    public float numberOfDivisions = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawLine(Camera.main.transform.InverseTransformPoint(target.position), numberOfDivisions);
    }

    public void EnableRenderer(bool enable)
    {
        lr.enabled = enable;
    }

    void DrawLine(Vector3 endPosition, float numberOfDivisions)
    {
        positions = new Vector3[(int)numberOfDivisions + 1];

        for (int ii = 0; ii < positions.Length; ii++)
        {
            positions[ii] = endPosition * (ii / numberOfDivisions);// + startPosition;
        }
        positions[positions.Length - 1] = endPosition;
        lr.positionCount = positions.Length;
        AddNoiseToPositions(noiseStrength);
        lr.SetPositions(positions);
    }

    void AddNoiseToPositions(float strength)
    {
        for(int ii = 1; ii < positions.Length - 1; ii++)
        {
            positions[ii] = new Vector3(positions[ii].x + Random.Range(-strength,strength), positions[ii].y + Random.Range(-strength, strength), positions[ii].z);
        }
    }
}
