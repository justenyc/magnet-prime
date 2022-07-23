using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            CheckpointManager.instance.SetCheckpoint(this.transform);
        }
    }
}
