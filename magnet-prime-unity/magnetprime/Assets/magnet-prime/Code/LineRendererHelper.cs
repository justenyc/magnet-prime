using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererHelper : MonoBehaviour
{
    public LineRenderer lr;
    public Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        lr.GetPositions(positions);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLastPosition(Vector3 position)
    {
        lr.GetPositions(positions);
    }

    public IEnumerator DisableLineRenderer(float time)
    {
        yield return new WaitForSeconds(time);
        lr.enabled = false;
    }
}
