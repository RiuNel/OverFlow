using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGestureDetection.Sample
{
    public class Smashable : MonoBehaviour
    {
        bool smashed = false;
        public Rigidbody originalRigidbody;
        public MeshRenderer[] nonSmashedRenderers;
        public MeshRenderer[] smashedRenderers;
        public Collider[] nonSmashedColliders;
        public Collider[] smashedColliders;
        public Collider[] fixedSmashedColliders;
        public Transform[] disableWhenSmashed;
        public UnityEngine.Events.UnityEvent OnSmash;
        public float breakForce = 100f;
        public float smashMagnitude = 2f;
        public List<Vector3> originalColliderPositions = new List<Vector3>();
        public List<Quaternion> originalColliderRotations = new List<Quaternion>();
        public AudioSource smashSound;
        public bool autoPopulateParts = false;

        void Start()
        {
            if (autoPopulateParts) {
                List<MeshRenderer> nonSmashedRenderersList = new List<MeshRenderer>();
                List<MeshRenderer> smashedRenderersList = new List<MeshRenderer>();
                List<Collider> nonSmashedCollidersList = new List<Collider>();
                List<Collider> smashedCollidersList = new List<Collider>();

                foreach (Transform child in GetComponentsInChildren<Transform>())
                {
                    MeshRenderer renderer = child.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        if (renderer.enabled)
                        {
                            nonSmashedRenderersList.Add(renderer);
                        }
                        else
                        {
                            smashedRenderersList.Add(renderer);
                        }
                    }

                    Collider collider = child.GetComponent<Collider>();
                    if (collider != null)
                    {
                        if (collider.enabled)
                        {
                            nonSmashedCollidersList.Add(collider);
                        }
                        else
                        {
                            smashedCollidersList.Add(collider);
                            originalColliderPositions.Add(collider.transform.localPosition);
                            originalColliderRotations.Add(collider.transform.localRotation);
                        }
                    }
                }

                nonSmashedRenderers = nonSmashedRenderersList.ToArray();
                smashedRenderers = smashedRenderersList.ToArray();
                nonSmashedColliders = nonSmashedCollidersList.ToArray();
                smashedColliders = smashedCollidersList.ToArray();
            }
        }

        public void Smash()
        {
            if (smashed)
            {
                return;
            }

            smashed = true;

            if (smashSound != null)
            {
                smashSound.Play();
            }

            foreach (Transform t in disableWhenSmashed)
            {
                t.gameObject.SetActive(false);
            }

            foreach (MeshRenderer r in nonSmashedRenderers)
            {
                r.enabled = false;
            }

            foreach (MeshRenderer r in smashedRenderers)
            {
                r.enabled = true;
            }

            foreach (Collider c in nonSmashedColliders)
            {
                c.enabled = false;
            }

            foreach (Collider c in smashedColliders)
            {
                c.enabled = true;
                // add a rigid body to the smashed pieces
                Rigidbody rb = c.gameObject.AddComponent<Rigidbody>();
                rb.mass = 0.1f;
                if (originalRigidbody != null)
                {
                    rb.velocity = originalRigidbody.velocity;
                }
                if (breakForce > 0f)
                {
                    rb.AddExplosionForce(breakForce, transform.position, 1f);
                }
            }

            foreach (Collider c in fixedSmashedColliders)
            {
                c.enabled = true;
                Rigidbody rb = c.gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }

            if (originalRigidbody != null)
            {
                originalRigidbody.isKinematic = true;
            }

            if (OnSmash != null)
            {
                OnSmash.Invoke();
            }
        }


        public void ResetObject()
        {
            smashed = false;

            foreach (Transform t in disableWhenSmashed)
            {
                t.gameObject.SetActive(true);
            }

            foreach (MeshRenderer r in nonSmashedRenderers)
            {
                r.enabled = true;
            }

            foreach (MeshRenderer r in smashedRenderers)
            {
                r.enabled = false;
            }

            foreach (Collider c in nonSmashedColliders)
            {
                c.enabled = true;
            }

            for (int x = 0; x < smashedColliders.Length; x++)
            {
                Collider c = smashedColliders[x];
                c.enabled = false;
                c.transform.localPosition = originalColliderPositions[x];
                c.transform.localRotation = originalColliderRotations[x];
                if (c.GetComponent<Rigidbody>() != null)
                {
                    Destroy(c.GetComponent<Rigidbody>());
                }
            }

            for (int x = 0; x < fixedSmashedColliders.Length; x++)
            {
                Collider c = fixedSmashedColliders[x];
                c.enabled = false;
                if (c.GetComponent<Rigidbody>() != null)
                {
                    Destroy(c.GetComponent<Rigidbody>());
                }
            }

            if (originalRigidbody != null)
            {
                originalRigidbody.isKinematic = false;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (smashMagnitude > 0f && collision.relativeVelocity.magnitude > smashMagnitude)
            {
                Smash();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            Smash();
        }
    }
}
