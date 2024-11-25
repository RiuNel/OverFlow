using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class GestureFishingPole : MonoBehaviour
    {
        public GestureDetection gestureDetection;
        public Transform fishingLineStart;
        public LineRenderer fishingLineRenderer;
        private bool lineOut = false, readyToBeReeledIn = false;
        public Rigidbody bobberRigidbody, fishingPoleRigidbody;
        public MeshRenderer fishingPoleMeshRenderer, bobberMeshRenderer;
        public Vector3 fishingPoleHandRotationOffset;
        public AudioSource audioSourceCasting, audioSourceReeling, audioSourceSplash;
        private SpringJoint bobberJoint;
        private string currentCastingSide;
        private InputDevice currentHandDevice;
        private Coroutine hapticCoroutine;

        void Start()
        {
            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();

            LoadGestures();
            fishingLineRenderer.positionCount = 2;

            if (bobberRigidbody != null)
            {
                bobberRigidbody.isKinematic = true;
            }

            fishingPoleMeshRenderer.enabled = false;
            bobberMeshRenderer.enabled = false;
        }

        void LoadGestures()
        {
            gestureDetection.ClearGestureEvents();

            GestureEvent throwLineRight = new GestureEvent();
            throwLineRight.gestureName = "throw fishing line right";
            throwLineRight.onGestureDetected = new UnityEngine.Events.UnityEvent();
            throwLineRight.onGestureDetected.AddListener(() => 
            {
                if (!lineOut)
                    ThrowFishingLine("right");
            });
            gestureDetection.AddGestureEvent(throwLineRight);

            GestureEvent throwLineLeft = new GestureEvent();
            throwLineLeft.gestureName = "throw fishing line left";
            throwLineLeft.onGestureDetected = new UnityEngine.Events.UnityEvent();
            throwLineLeft.onGestureDetected.AddListener(() => 
            {
                if (!lineOut)
                    ThrowFishingLine("left");
            });
            gestureDetection.AddGestureEvent(throwLineLeft);

            GestureEvent rightHandReelIn = new GestureEvent();
            rightHandReelIn.gestureName = "reeling in right hand";
            rightHandReelIn.onGestureDetected = new UnityEngine.Events.UnityEvent();
            rightHandReelIn.onGestureDetected.AddListener(() => 
            {
                if (lineOut)
                    ReelIn("right");
            });
            gestureDetection.AddGestureEvent(rightHandReelIn);

            GestureEvent leftHandReelIn = new GestureEvent();
            leftHandReelIn.gestureName = "reeling in left hand";
            leftHandReelIn.onGestureDetected = new UnityEngine.Events.UnityEvent();
            leftHandReelIn.onGestureDetected.AddListener(() => 
            {
                if (lineOut)
                    ReelIn("left");
            });
            gestureDetection.AddGestureEvent(leftHandReelIn);
        }

        void FixedUpdate()
        {
            if (lineOut && readyToBeReeledIn )
            {
                if (gestureDetection.GetCurrentGesture() == "reeling in right hand" || gestureDetection.GetCurrentGesture() == "reeling in left hand")
                {
                    if (audioSourceReeling.isPlaying == false)
                    {
                        audioSourceReeling.Play();
                    }
                }
                else
                {
                    audioSourceReeling.Stop();
                }
            }
            else
            {
                audioSourceReeling.Stop();
            }
        }

        void LateUpdate()
        {
            if (lineOut)
            {
                fishingLineRenderer.enabled = true;
                fishingLineRenderer.SetPosition(0, fishingLineStart.position);
                fishingLineRenderer.SetPosition(1, bobberRigidbody.transform.position);

                // Adjust bobber drag based on height
                if (bobberRigidbody.transform.position.y <= 0f && !readyToBeReeledIn)
                {
                    bobberRigidbody.drag = 2;
                    CreateSpringJoint();
                }
                else
                {
                    bobberRigidbody.drag = 0;
                }

                // Track the fishing pole to the correct hand
                if (currentCastingSide == "right")
                {
                    fishingPoleRigidbody.transform.position = gestureDetection.leftHandTransform.position;
                    fishingPoleRigidbody.transform.rotation = gestureDetection.leftHandTransform.rotation * Quaternion.Euler(fishingPoleHandRotationOffset);
                }
                else if (currentCastingSide == "left")
                {
                    fishingPoleRigidbody.transform.position = gestureDetection.rightHandTransform.position;
                    fishingPoleRigidbody.transform.rotation = gestureDetection.rightHandTransform.rotation * Quaternion.Euler(fishingPoleHandRotationOffset);
                }
            }
            else
            {
                readyToBeReeledIn = false;
                fishingLineRenderer.enabled = false;
                fishingPoleMeshRenderer.enabled = false;
            }
        }

        void ThrowFishingLine(string castOnSide)
        {
            audioSourceCasting.Play();
            // Remove any existing joints
            if (bobberJoint != null)
            {
                Destroy(bobberJoint);
                bobberJoint = null;
            }

            readyToBeReeledIn = false;

            // Enable the fishing pole and track it to the correct hand
            fishingPoleMeshRenderer.enabled = true;
            bobberMeshRenderer.enabled = true;

            // Enable physics for the bobber
            bobberRigidbody.isKinematic = false;
            bobberRigidbody.velocity = Vector3.zero;
            bobberRigidbody.angularVelocity = Vector3.zero;

            // Cast the fishing line in the direction the main camera is facing
            Vector3 castDirection = Camera.main.transform.forward + Vector3.up * 0.2f;
            bobberRigidbody.transform.position = fishingLineStart.position;
            bobberRigidbody.AddForce(castDirection * 2, ForceMode.Impulse);

            lineOut = true;
            currentCastingSide = castOnSide;
            currentHandDevice = gestureDetection.GetHandDevice(castOnSide);

            if (currentHandDevice.isValid)
            {
                currentHandDevice.SendHapticImpulse(0, 0.1f, 0.1f); // Soft haptic feedback
            }
        }

        void CreateSpringJoint()
        {
            bobberJoint = bobberRigidbody.gameObject.AddComponent<SpringJoint>();
            bobberJoint.connectedBody = fishingPoleRigidbody;
            bobberJoint.autoConfigureConnectedAnchor = false;
            bobberJoint.connectedAnchor = fishingLineStart.position;
            bobberJoint.anchor = Vector3.zero;
            bobberJoint.spring = 100f;
            bobberJoint.damper = 10f;
            bobberJoint.maxDistance = Vector3.Distance(bobberRigidbody.position, fishingLineStart.position);
            readyToBeReeledIn = true;
            audioSourceSplash.Play();
        }

        void ReelIn(string reelingHand)
        {
            currentHandDevice = gestureDetection.GetHandDevice(reelingHand);

            if (bobberJoint == null) {
                return;
            }

            if (currentHandDevice.isValid)
            {
                currentHandDevice.SendHapticImpulse(0, 0.1f, 0.1f); // Soft haptic feedback
            }

            // Shorten the spring joint distance
            bobberJoint.maxDistance = Mathf.Max(0, bobberJoint.maxDistance - 0.25f);

            if (bobberJoint.maxDistance <= 0.5f)
            {
                ReeledIn();
            }
        }

        void ReeledIn() 
        {
            lineOut = false;
            readyToBeReeledIn = false;
            fishingLineRenderer.enabled = false;
            fishingPoleMeshRenderer.enabled = false;
            bobberMeshRenderer.enabled = false;
        }
    }
}
