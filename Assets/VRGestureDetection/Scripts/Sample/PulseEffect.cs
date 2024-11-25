using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGestureDetection.Sample
{
    public class PulseEffect : MonoBehaviour
    {
        float life = 0f;
        public float maxLife = 4f;
        public float pulseSpeed = 2f;
        void Update()
        {
            // move this forward at a constant rate
            transform.position += transform.forward * Time.deltaTime * pulseSpeed;

            life += Time.deltaTime;
            if (life > maxLife)
            {
                Destroy(gameObject);
            }
        }
    }
}