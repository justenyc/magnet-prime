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
        instance = instance ?? this;
        playerHealth = FindObjectOfType<Health>();
        playerHealth.DieAction += OnPlayerDie;
    }
    
    void OnPlayerDie()
    {
        playerHealth.gameObject.transform.position = currentCheckpoint.position;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
    }
}
