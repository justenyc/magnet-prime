using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ParticleShotListener : MonoBehaviour
{
    [SerializeField] FirstPersonController player;
    [SerializeField] GameObject particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            Debug.LogError(this.name + " Says: Player not found > Using FindObjectOfType<FirstPersonController>()");
            player = FindObjectOfType<FirstPersonController>();
            Debug.Log("Player is now " + player);
        }

        player.InvokeShoot += ShootListener;
    }

    private void OnDestroy()
    {
        player.InvokeShoot -= ShootListener;
    }

    void ShootListener(GameObject objectHit)
    {
        ParticleSystem ps = particleSystem.GetComponent<ParticleSystem>();
        ps.Play();
    }
}
