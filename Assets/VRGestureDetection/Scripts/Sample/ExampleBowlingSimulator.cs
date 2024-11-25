using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class ExampleBowlingSimulator : MonoBehaviour
    {
        public GestureDetection gestureDetection;
        public GameObject bowlingBallPrefab;
        public float minimumReleaseSpeed = 2.0f;  // Minimum speed threshold
        public float verticalVelocityReduction = 0.5f;  // Reduction factor for vertical velocity
        float cooldown = 0f;

        void Start()
        {
            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();

            LoadGestures();
        }

        public void LoadGestures()
        {
            gestureDetection.ClearGestureEvents();

            GestureEvent rightHandThrowEvent = new GestureEvent();
            rightHandThrowEvent.gestureName = "bowling right";
            rightHandThrowEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            rightHandThrowEvent.onGestureDetected.AddListener(() => ThrowBall("right"));
            gestureDetection.AddGestureEvent(rightHandThrowEvent);

            GestureEvent leftHandThrowEvent = new GestureEvent();
            leftHandThrowEvent.gestureName = "bowling left";
            leftHandThrowEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            leftHandThrowEvent.onGestureDetected.AddListener(() => ThrowBall("left"));
            gestureDetection.AddGestureEvent(leftHandThrowEvent);

            GestureEvent bothHandsThrowEvent = new GestureEvent();
            bothHandsThrowEvent.gestureName = "bowling two hands";
            bothHandsThrowEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            bothHandsThrowEvent.onGestureDetected.AddListener(() => ThrowBall("both"));
            gestureDetection.AddGestureEvent(bothHandsThrowEvent);
        }

        void Update()
        {
            if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
            }
        }

        public void ThrowBall(string hand)
        {
            if (cooldown > 0f)
            {
                return;
            }

            GameObject bowlingBall = Instantiate(bowlingBallPrefab);

            if (hand == "right" || hand == "left")
            {
                InputDevice handDevice = gestureDetection.GetHandDevice(hand);
                Transform handTransform = gestureDetection.GetHandTransform(hand);
                bowlingBall.transform.position = handTransform.position + Camera.main.transform.forward * 0.5f + Vector3.up * 0.25f;

                Vector3 deviceVelocity;
                if (handDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity))
                {
                    ApplyVelocity(bowlingBall, deviceVelocity);
                }
            }
            else
            {
                // both hands
                InputDevice rightHandDevice = gestureDetection.GetHandDevice("right");
                Transform rightHandTransform = gestureDetection.GetHandTransform("right");
                InputDevice leftHandDevice = gestureDetection.GetHandDevice("left");
                Transform leftHandTransform = gestureDetection.GetHandTransform("left");

                bowlingBall.transform.position = (rightHandTransform.position + leftHandTransform.position) / 2 + Camera.main.transform.forward * 0.5f + Vector3.up * 0.5f;

                Vector3 rightDeviceVelocity;
                Vector3 leftDeviceVelocity;
                if (rightHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out rightDeviceVelocity) &&
                    leftHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out leftDeviceVelocity))
                {
                    Vector3 averageVelocity = (rightDeviceVelocity + leftDeviceVelocity) / 2;
                    ApplyVelocity(bowlingBall, averageVelocity);
                }
            }

            cooldown = 1f;
        }

        private void ApplyVelocity(GameObject bowlingBall, Vector3 velocity)
        {
            // Enforce minimum release speed
            if (velocity.magnitude < minimumReleaseSpeed)
            {
                velocity = velocity.normalized * minimumReleaseSpeed;
            }

            // Flatten the vertical component
            velocity.y *= verticalVelocityReduction;

            bowlingBall.GetComponent<Rigidbody>().velocity = velocity * 1.5f;
        }
    }
}
