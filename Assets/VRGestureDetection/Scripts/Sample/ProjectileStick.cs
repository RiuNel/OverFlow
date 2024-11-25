using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStick : MonoBehaviour
{
    Rigidbody rb;
    bool _stuck = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_stuck || collision.gameObject.tag == "ResetObject")
        {
            return;
        }

        Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRb != null && !otherRb.isKinematic)
        {
            otherRb.velocity = rb.velocity;
        }

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetParent(collision.transform);

        _stuck = true;
    }
}
