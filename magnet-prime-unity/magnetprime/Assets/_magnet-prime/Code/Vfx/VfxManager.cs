using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public static VfxManager instance;
    public GameObject[] vfxObjects;
    public Dictionary<string, GameObject> vfxDict = new Dictionary<string, GameObject>();

    void Start()
    {
        instance = instance ?? this;
        PopDict();
    }

    void PopDict()
    {
        if(vfxObjects.Length > 0)
        {
            foreach(GameObject go in vfxObjects)
            {
                vfxDict.Add(go.name, go);
            }
            return;
        }
        Debug.LogWarning("vfxManager empty");
        return;
    }

    public GameObject GetEffect(string name)
    {
        return vfxDict[name];
    }
}
