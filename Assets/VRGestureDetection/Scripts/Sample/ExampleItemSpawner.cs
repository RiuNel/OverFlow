using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class ExampleItemSpawner : MonoBehaviour
    {
        public GestureDetection gestureDetection;

        public GameObject redBallPrefab, blueBallPrefab, greyBallPrefab;

        GameObject[] boxes;

        Vector3[] boxPositions;
        Quaternion[] boxRotations;

        void Start()
        {
            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();

            boxes = GameObject.FindGameObjectsWithTag("Box");

            boxPositions = new Vector3[boxes.Length];
            boxRotations = new Quaternion[boxes.Length];

            for (int i = 0; i < boxes.Length; i++)
            {
                boxPositions[i] = boxes[i].transform.position;
                boxRotations[i] = boxes[i].transform.rotation;
            }

            LoadGestures();
        }

        public void LoadGestures()
        {
            gestureDetection.ClearGestureEvents();

            GestureEvent rightHandThrowEvent = new GestureEvent();
            rightHandThrowEvent.gestureName = "right hand throw overhand";
            rightHandThrowEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            
            // this is where we assign the function to be called when the gesture is detected
            rightHandThrowEvent.onGestureDetected.AddListener(() => ThrowBall("red"));
            gestureDetection.AddGestureEvent(rightHandThrowEvent);

            GestureEvent leftHandThrowEvent = new GestureEvent();
            leftHandThrowEvent.gestureName = "left hand throw overhand";
            leftHandThrowEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();

            leftHandThrowEvent.onGestureDetected.AddListener(() => ThrowBall("blue"));
            gestureDetection.AddGestureEvent(leftHandThrowEvent);


            GestureEvent twoHandThrowEvent = new GestureEvent();
            twoHandThrowEvent.gestureName = "clap";
            twoHandThrowEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();

            twoHandThrowEvent.onGestureDetected.AddListener(() => ThrowBall("grey"));
            gestureDetection.AddGestureEvent(twoHandThrowEvent);


            GestureEvent crossArmsEvent = new GestureEvent();
            crossArmsEvent.gestureName = "cross arms";
            crossArmsEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();

            crossArmsEvent.onGestureDetected.AddListener(() => ResetScene());
            gestureDetection.AddGestureEvent(crossArmsEvent);

        }

        void ResetScene()
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].transform.position = boxPositions[i];
                boxes[i].transform.rotation = boxRotations[i];
            }

            GameObject[] thrownObjects = GameObject.FindGameObjectsWithTag("ResetObject");
            foreach (GameObject thrownObject in thrownObjects)
            {
                Destroy(thrownObject);
            }
        }

        // Simply use the gesture detection system to get the input device and trigger haptic feedback
        void ThrowBall(string ballColor)
        {
            GameObject ball = null;
            if (ballColor == "red")
            {
                ball = Instantiate(redBallPrefab, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
                if (gestureDetection != null && gestureDetection.GetHandDevice("right") != null &&  gestureDetection.GetHandDevice("right").isValid)
                {
                    gestureDetection.GetHandDevice("right").SendHapticImpulse(0, 0.5f, 0.1f);
                }
            }
            else if (ballColor == "blue")
            {
                ball = Instantiate(blueBallPrefab, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
                if (gestureDetection != null && gestureDetection.GetHandDevice("left") != null && gestureDetection.GetHandDevice("left").isValid)
                {
                    gestureDetection.GetHandDevice("left").SendHapticImpulse(0, 0.5f, 0.1f);
                }
            }
            else if (ballColor == "grey")
            {
                ball = Instantiate(greyBallPrefab, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
                if (gestureDetection != null && gestureDetection.GetHandDevice("right") != null && gestureDetection.GetHandDevice("right").isValid)
                {
                    gestureDetection.GetHandDevice("right").SendHapticImpulse(0, 0.5f, 0.1f);
                }
                if (gestureDetection != null && gestureDetection.GetHandDevice("left") != null && gestureDetection.GetHandDevice("left").isValid)
                {
                    gestureDetection.GetHandDevice("left").SendHapticImpulse(0, 0.5f, 0.1f);
                }
            }

            ball.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 500);
        }
    }
}