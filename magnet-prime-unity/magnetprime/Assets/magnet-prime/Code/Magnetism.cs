using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Magnetism : MonoBehaviour
{
    public virtual void OnPlayerPolarize(FirstPersonController player)
    {
        Debug.Log(player.name + " has used polarize on " + this.name);
    }
}
