using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGestureDetection.Sample
{
    public class ObjectResetter : MonoBehaviour
    {
        public Rigidbody[] bodiesToReset;
        public float bodiesResetAfter = 5f;
        public AudioSource hitSound;
        Vector3[] bodyPositions;
        Quaternion[] bodyRotations;

        List<Collider> destroyColliders = new List<Collider>();

        bool _bodiesKnockedDown = false;
        float bodyResetTime = 0f;

        void Start()
        {
            bodyPositions = new Vector3[bodiesToReset.Length];
            bodyRotations = new Quaternion[bodiesToReset.Length];

            for (int i = 0; i < bodiesToReset.Length; i++)
            {
                bodyPositions[i] = bodiesToReset[i].transform.position;
                bodyRotations[i] = bodiesToReset[i].transform.rotation;
            }

        }

        void Update()
        {
            if (_bodiesKnockedDown)
            {
                bodyResetTime += Time.deltaTime;
                if (bodyResetTime >= bodiesResetAfter)
                {
                    for (int i = 0; i < bodiesToReset.Length; i++)
                    {
                        bodiesToReset[i].transform.position = bodyPositions[i];
                        bodiesToReset[i].transform.rotation = bodyRotations[i];
                        bodiesToReset[i].velocity = Vector3.zero;
                        bodiesToReset[i].angularVelocity = Vector3.zero;
                    }
                    _bodiesKnockedDown = false;
                    bodyResetTime = 0f;

                    foreach (Collider col in destroyColliders)
                    {
                        Destroy(col.gameObject);
                    }
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "ResetObject")
            {
                destroyColliders.Add(other);
                _bodiesKnockedDown = true;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "ResetObject")
            {
                if (destroyColliders.Contains(other))
                {
                    return;
                }
                destroyColliders.Add(other);
                _bodiesKnockedDown = true;
            }
        }
    }
}