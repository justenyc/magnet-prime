using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public float damage = 50f;
    public float moveSpeed;
    public float trackSpeed;
    public Transform target;
    public bool track;

    // Start is called before the first frame update
    void Start()
    {
        Deactivate(lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        TrackTarget();
        Movement();
    }

    void TrackTarget()
    {
        if (target != null && track == true)
        {
            Vector3 dirFromSelfToTarget = (transform.position - target.transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirFromSelfToTarget);
            if (dot > 0)
            {
                track = false;
            }

            Vector3 rot = transform.rotation.eulerAngles;
            Vector3 direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction.normalized, Vector3.up), trackSpeed * Time.deltaTime);
        }
    }

    private void Movement()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void Deactivate(float delay)
    {
        Destroy(this.gameObject, delay);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
            //Debug.Log(health.gameObject.name);
        }
    }
}