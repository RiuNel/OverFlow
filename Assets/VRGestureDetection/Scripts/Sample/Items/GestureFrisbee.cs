using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class GestureFrisbee : MonoBehaviour
        {
        public float liftCoefficient = 0.1f;  // Coefficient for the lift force
        public float dragCoefficient = 0.1f;  // Coefficient for the drag force
        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
        }

        void FixedUpdate()
        {
            ApplyLift();
            ApplyDrag();
        }

        private void ApplyLift()
        {
            // Lift force is perpendicular to the velocity and the up vector of the frisbee
            Vector3 liftDirection = Vector3.Cross(rb.velocity, transform.right).normalized;
            float speed = rb.velocity.magnitude;
            float liftForce = liftCoefficient * speed * speed;
            rb.AddForce(liftDirection * liftForce);
        }

        private void ApplyDrag()
        {
            // Drag force is opposite to the velocity
            float speed = rb.velocity.magnitude;
            float dragForce = dragCoefficient * speed * speed;
            rb.AddForce(-rb.velocity.normalized * dragForce);
        }
    }
}