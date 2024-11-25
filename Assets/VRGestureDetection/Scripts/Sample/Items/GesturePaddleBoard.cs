using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class GesturePaddleBoard : MonoBehaviour
    {
        public GestureDetection gestureDetection;
        public Rigidbody paddleboard;
        public float paddleForce = 0.3f;
        public AudioSource splashLeftSide, splashRightSide;
        public AudioClip[] splashSounds;

        void Start()
        {
            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();


            LoadGestures();
        }

        public void LoadGestures()
        {
            gestureDetection.ClearGestureEvents();

            GestureEvent strokeRightEvent = new GestureEvent();
            strokeRightEvent.gestureName = "paddleboard stroke right";
            strokeRightEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            strokeRightEvent.onGestureDetected.AddListener(() => 
            {
                SplashSound("right");
                MovePaddleboard(new Vector3(-0.2f, 0f, 0.5f), -0.05f, "right");
            });
            gestureDetection.AddGestureEvent(strokeRightEvent);

            GestureEvent strokeLeftEvent = new GestureEvent();
            strokeLeftEvent.gestureName = "paddleboard stroke left";
            strokeLeftEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            strokeLeftEvent.onGestureDetected.AddListener(() => 
            {
                SplashSound("left");
                MovePaddleboard(new Vector3(0.2f, 0f, 0.5f), 0.05f, "left");
            });
            gestureDetection.AddGestureEvent(strokeLeftEvent);

/*
            GestureEvent reverseStrokeRightEvent = new GestureEvent();
            reverseStrokeRightEvent.gestureName = "paddleboard reverse stroke right";
            reverseStrokeRightEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            reverseStrokeRightEvent.onGestureDetected.AddListener(() => 
            {
                SplashSound("right");
                MovePaddleboard(new Vector3(0.2f, 0f, -0.5f), 0.08f, "right");
            });            
            gestureDetection.AddGestureEvent(reverseStrokeRightEvent);

            GestureEvent reverseStrokeLeftEvent = new GestureEvent();
            reverseStrokeLeftEvent.gestureName = "paddleboard reverse stroke left";
            reverseStrokeLeftEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            reverseStrokeLeftEvent.onGestureDetected.AddListener(() => 
            {
                SplashSound("left");
                MovePaddleboard(new Vector3(-0.2f, 0f, -0.5f), -0.08f, "left");
            });
            gestureDetection.AddGestureEvent(reverseStrokeLeftEvent);
*/
        }

        void MovePaddleboard(Vector3 direction, float rotationNudge = 0f, string hand = "right")
        {
            paddleboard.AddForce((paddleboard.transform.forward + direction) * paddleForce, ForceMode.Impulse);
            paddleboard.AddTorque(Vector3.up * rotationNudge, ForceMode.Impulse);

            // Simply get the input device from the gesture detection system to do things like trigger haptic feedback
            if (hand == "right")
            {
                TriggerHapticFeedback(gestureDetection.GetHandDevice("right"));
            }
            else
            {
                TriggerHapticFeedback(gestureDetection.GetHandDevice("left"));
            }
        }

        void SplashSound(string side)
        {
            if (side == "right")
            {
                if (splashRightSide.isPlaying)
                    splashRightSide.Stop();

                splashRightSide.clip = splashSounds[Random.Range(0, splashSounds.Length)];
                splashRightSide.Play();
            }
            else
            {
                if (splashLeftSide.isPlaying)
                    splashLeftSide.Stop();

                splashLeftSide.clip = splashSounds[Random.Range(0, splashSounds.Length)];
                splashLeftSide.Play();
            }
        }

        void TriggerHapticFeedback(InputDevice hand)
        {
            if (hand != null && hand.isValid)
            {
                hand.SendHapticImpulse(0, 0.5f, 0.1f);
            }
        }
    }
}
