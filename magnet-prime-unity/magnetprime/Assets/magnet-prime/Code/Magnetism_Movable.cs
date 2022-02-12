using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Charge))]
[RequireComponent(typeof(Rigidbody))]
public class Magnetism_Movable : Magnetism
{
    public Charge myCharge;
    public bool grabbable;
    public Vector3 direction;
    public Rigidbody rigidBody;
    public float polarizeCD = 5f;
    public float polarizeCDTime;
    public bool dragToPlayer = false;
    float polarizeStrength = 1;
    Vector3 startingScale;
    private void Start()
    {
        startingScale = transform.localScale;
        polarizeCDTime = polarizeCD;
        if (myCharge == null)
        {
            Debug.LogError(this.name + " Says: myCharge not found > Using GetComponent<Charge>()");
            myCharge = this.GetComponent<Charge>();
            Debug.Log("myCharge is now " + myCharge);
        }

        if (rigidBody == null)
        {
            Debug.LogError(this.name + " Says: RigidBody not found > Using GetComponent<RigidBody>()");
            rigidBody = this.GetComponent<Rigidbody>();
            Debug.Log("RigidBody is now " + myCharge);
        }

        FirstPersonController player = FindObjectOfType<FirstPersonController>();
        player.InvokePolarize += OnPlayerPolarize;
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
            }
            else if (fpc.polarity * myCharge.GetPolarity() > 0)
            {
                SetDragToPlayer(false);
                rigidBody.AddForce((transform.position - player.transform.position) * fpc.polarizeStrength, ForceMode.Impulse);
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
            scaleTo = new Vector3(1, 1, 1);
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
}