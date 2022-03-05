using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Magnetism : MonoBehaviour
{
    private void Start()
    {

    }

    public virtual void OnPlayerPolarize(GameObject player, GameObject objectHit)
    {
        Debug.Log(player.name + " has used polarize on " + this.name);
    }
}
