using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class ExampleGestureLoader : MonoBehaviour
    {
        public GestureDetection gestureDetection;
        public TextMeshProUGUI gestureResultsText;

        void Start()
        {
            LoadGestures();
        }

        string[] gestures = new string[] { "right hand throw overhand", "left hand throw overhand", "jazz hands", "cross arms" };

        public void LoadGestures()
        {
            gestureDetection.ClearGestureEvents();

            foreach (string gesture in gestures)
            {
                GestureEvent gestureEvent = new GestureEvent();
                gestureEvent.gestureName = gesture;
                gestureEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
                gestureEvent.onGestureDetected.AddListener(() => SetLatestGesture(gesture));
                gestureDetection.AddGestureEvent(gestureEvent);
            }
        }

        void SetLatestGesture(string gestureName)
        {
            gestureResultsText.text = gestureName + "\n" + gestureResultsText.text;
        }
    }
}
