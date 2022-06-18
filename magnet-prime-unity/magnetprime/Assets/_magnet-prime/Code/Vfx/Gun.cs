using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Gun : MonoBehaviour
{
    public FirstPersonController fpc;
    public AudioSource aSource;
    [Space(10)]
    public Material posMat;
    public ParticleSystem posParticle;
    public Material negMat;
    public ParticleSystem negParticle;

    private void Start()
    {
        fpc.InvokeShoot += ShootListener;
    }

    void ShootListener(GameObject go = null)
    {
        if (fpc.lastShot)
        {
            posParticle.Play();
            SfxManager.instance.PlayFromSource(aSource, "MagnetGun_Shot1");
            return;
        }
        negParticle.Play();
        SfxManager.instance.PlayFromSource(aSource, "MagnetGun_Shot2");
        return;
    }
}
