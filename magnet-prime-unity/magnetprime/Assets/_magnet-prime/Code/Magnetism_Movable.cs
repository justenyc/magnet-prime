using System;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Magnetism_Movable : Magnetism
{
    public startTransform startingTransform;
    public Charge myCharge;
    public bool grabbable;
    public Vector3 direction;
    public Rigidbody rigidBody;
    public float polarizeCD = 5f;
    public float polarizeCDTime;
    public bool dragToPlayer = false;
    float polarizeStrength = 1;
    public bool beingMagnetized = false;
    [SerializeField] ParticleSystem posEffect = null;
    [SerializeField] ParticleSystem negEffect = null;

    public AudioSource aSource { get; private set; }
    Vector3 startingScale;

    public Action EnteredBoxField;

    private void Start()
    {
        startingTransform.startPos = this.transform.position;
        startingTransform.startRot = this.transform.rotation;
        startingTransform.startScale = this.transform.localScale;
        aSource = this.GetComponent<AudioSource>();

        startingScale = transform.localScale;
        polarizeCDTime = polarizeCD;

        EnteredBoxField += ResetValues;

        myCharge = this.GetComponent<Charge>() ?? myCharge; ;
        rigidBody = this.GetComponent<Rigidbody>() ?? rigidBody;

        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        player.InvokePolarize += OnPlayerPolarize;
        myCharge.polarityChange += OnPolarityChange;
        OnPolarityChange(myCharge.GetPolarity());
    }

    private void FixedUpdate()
    {
        if (polarizeCDTime > 0)
        {
            polarizeCDTime = Mathf.Clamp(polarizeCDTime - Time.fixedDeltaTime, 0, polarizeCD);
            if (dragToPlayer)
            {
                rigidBody.velocity = Vector3.zero;
                transform.position = Vector3.MoveTowards(transform.position, direction, Time.fixedDeltaTime * polarizeStrength * 2);
            }
        }
        else
        {
            dragToPlayer = false;
        }
    }
    public int GetPolarity()
    {
        return myCharge.GetPolarity();
    }

    public override void OnPlayerPolarize(GameObject player, GameObject objectHit)
    {
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        int action = fpc.polarity * myCharge.GetPolarity();

        if (objectHit == this.gameObject)
        {
            if (fpc.polarity * myCharge.GetPolarity() < 0 && grabbable == true)
            {
                dragToPlayer = true;
                polarizeStrength = fpc.polarizeStrength;
                direction = fpc.transform.position + Vector3.up + fpc.transform.forward * fpc.grabDistance;
                polarizeCDTime = polarizeCD;
                SfxManager.instance.SetVolume(SfxManager.instance.mainSource, 1);
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "Polarize_Pull", oneshot: true);
            }
            else if (fpc.polarity * myCharge.GetPolarity() > 0)
            {
                SetDragToPlayer(false);
                rigidBody.AddForce((transform.position - player.transform.position) * fpc.polarizeStrength, ForceMode.Impulse);
                SfxManager.instance.SetVolume(SfxManager.instance.mainSource, 1);
                SfxManager.instance.PlayFromSource(SfxManager.instance.mainSource, "Polarize_Push", oneshot: true);
            }
        }
    }

    public void SetDragToPlayer(bool b)
    {
        dragToPlayer = b;
    }

    public IEnumerator LerpScale(bool direction)
    {
        Vector3 scaleTo;
        if (direction)
        {
            scaleTo = new Vector3(1f, 1f, 1f);
        }
        else
        {
            scaleTo = this.startingScale;
        }
        float elapsedTime = 0;
        Vector3 startingScale = transform.localScale;
        while (elapsedTime < 1)
        {
            transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / 0.1f));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = scaleTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.gameObject.layer == LayerMask.NameToLayer("BoxForceField") || other.gameObject.layer == LayerMask.NameToLayer("AbsoluteDeath"))
        {
            if (EnteredBoxField != null)
                EnteredBoxField();
        }
    }

    public void ResetValues()
    {
        try
        {
            this.transform.position = startingTransform.startPos;
            this.transform.rotation = startingTransform.startRot;
            this.transform.localScale = startingTransform.startScale;
            rigidBody.velocity = Vector3.zero;
            dragToPlayer = false;
            beingMagnetized = false;

            SfxManager.instance.PlayFromSource(aSource, "BoxRespawn");
        }
        catch
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.name);
        if (collision.impulse.magnitude > 10f)
        {
            float vol = 0.25f * rigidBody.velocity.magnitude;
            SfxManager.instance.SetVolume(aSource, Mathf.Clamp(vol, 0.25f, 1));
            SfxManager.instance.RandomizePitch(aSource, 0.1f, 0.5f);
            SfxManager.instance.PlayFromSource(aSource, "Box_clang", oneshot: true);
        }
        else if (collision.impulse.magnitude > 1f)
        {
            SfxManager.instance.SetVolume(aSource, 0.25f);
            SfxManager.instance.RandomizePitch(aSource, 1f, 1.5f);
            SfxManager.instance.PlayFromSource(aSource, "Box Drop", oneshot: true);
        }
    }

    public void OnPolarityChange(int polarity)
    {
        //Debug.Log($"{this.name}: OnPolarityChange() called");
        if (polarity > 0)
        {
            posEffect?.Play();
            negEffect?.Stop();
            return;
        }
        else if (polarity < 0)
        {
            posEffect?.Stop();
            negEffect?.Play();
            return;
        }
        posEffect?.Stop();
        negEffect?.Stop();
    }

    public struct startTransform
    {
        public Vector3 startPos;
        public Quaternion startRot;
        public Vector3 startScale;
    };
}