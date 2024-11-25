using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class GestureBow : MonoBehaviour
    {
        public LineRenderer bowString;
        public GameObject arrowPrefab;
        public Transform stringAnchorTop, stringAnchorBottom;
        Transform bowHoldingHand, stringHoldingHand;
        Rigidbody heldArrow = null;
        InputDevice stringHandDevice;
        GestureCombatSimulator combatSimulator;

        void Start()
        {
            combatSimulator = FindObjectOfType<GestureCombatSimulator>();
        }

        public void SetBowHoldHand(Transform hand)
        {
            bowHoldingHand = hand;
        }

        public void SetStringHoldHand(Transform hand)
        {
            stringHoldingHand = hand;
        }

        public void SetStringHoldDevice(InputDevice device)
        {
            stringHandDevice = device;
        }

        void Update()
        {
            if (bowHoldingHand == null || stringHoldingHand == null)
                return;

            transform.position = bowHoldingHand.position;
            transform.rotation = bowHoldingHand.rotation;
            bowString.enabled = true;
            bowString.SetPosition(0, stringAnchorTop.position);
            bowString.SetPosition(1, stringHoldingHand.position);
            bowString.SetPosition(2, stringAnchorBottom.position);

            if (heldArrow != null)
            {
                heldArrow.transform.position = stringHoldingHand.position;

                Vector3 direction = bowHoldingHand.position - heldArrow.transform.position;
                heldArrow.transform.rotation = Quaternion.LookRotation(direction);
            }

            // Check if the grip button is released
            if (stringHandDevice.isValid && stringHandDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue) && !gripValue)
            {
                ReleaseArrow();
            }
        }

        public void SpawnArrow()
        {
            GameObject arrow = Instantiate(arrowPrefab, stringHoldingHand.position, stringHoldingHand.rotation);
            heldArrow = arrow.GetComponent<Rigidbody>();
        }

        void ReleaseArrow()
        {
            if (heldArrow != null)
            {
                AudioSource arrowSound = heldArrow.gameObject.GetComponent<AudioSource>();
                if (arrowSound != null)
                {
                    arrowSound.Play();
                }

                heldArrow.isKinematic = false;
                heldArrow.AddForce((bowHoldingHand.position - stringHoldingHand.position) * 20f, ForceMode.VelocityChange); // Adjust force as needed
                heldArrow = null;
                combatSimulator.bowDrawn = false;
                Destroy(gameObject); // Destroy the bow object after releasing the arrow
            }
        }
    }
}