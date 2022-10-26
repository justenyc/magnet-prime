using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Highlight : MonoBehaviour
{
    [SerializeField] Material _highlightMaterial = null;

    public bool isAimedAt = false;

    private void Start()
    {
        Material[] mats = this.GetComponent<Renderer>().materials;
        foreach(Material mat in mats)
        {
            if(mat.name.Contains("Outline"))
            {
                _highlightMaterial = mat;
                return;
            }
        }
        Debug.LogWarning($"<color=yellow>_highlightMaterial not found on {this.name}");
    }

    public void HighLight(bool on)
    {
        _highlightMaterial.SetFloat("OutlineAlpha", on ? 1 : 0);
    }
}
