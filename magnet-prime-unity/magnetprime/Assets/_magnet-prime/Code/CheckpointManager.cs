using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    public Transform defaultCheckpoint;
    public Transform currentCheckpoint;
    public Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        playerHealth = FindObjectOfType<Health>();
    }
    
    public void SendPlayerToCheckPoint()
    {
        playerHealth.gameObject.transform.position = currentCheckpoint != null ? currentCheckpoint.position : defaultCheckpoint.position;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }
}
