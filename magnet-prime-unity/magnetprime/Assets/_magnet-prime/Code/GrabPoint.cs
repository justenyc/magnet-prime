using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabPoint : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        transform.DOKill();
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        ResetPosition();
    }

    public void ResetPosition()
    {
        transform.DOKill();
        transform.DOLocalMove(new Vector3(0, 0, 3), 0.5f).SetDelay(0.1f);
    }
}
