using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Magnetism_Enemy : Magnetism_Movable
{
    public int GetPolarity()
    {
        return myCharge.GetPolarity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse.magnitude);
        if (collision.impulse.magnitude > 10f)
        {
            float vol = 0.25f * rigidBody.velocity.magnitude;
            SfxManager.instance.SetVolume(aSource, Mathf.Clamp(vol, 0.25f, 1));
            SfxManager.instance.RandomizePitch(aSource, 0.1f, 0.5f);
            SfxManager.instance.PlayFromSource(aSource, "Box_clang", oneshot: true);
        }
    }
}
