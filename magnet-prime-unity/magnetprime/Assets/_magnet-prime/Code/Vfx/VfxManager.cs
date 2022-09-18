using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VfxManager", menuName = "ScriptableObjects/VfxManager", order = 1)]
public class VfxManager : ScriptableObject
{
    public static VfxManager instance;
    public GameObject[] vfxObjects;
    public Dictionary<string, GameObject> vfxDict;

    public void Initialize()
    {
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
