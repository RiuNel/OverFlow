using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VRGestureDetection.Core
{
    /// <summary>
    /// Helper class to set the head and hand transforms for the GestureDetection instance on scene changes
    public class GestureTransformHelper : MonoBehaviour
    {
        public Transform head, handLeft, handRight;
        public Transform[] forceObjectsActive;
        GestureDetection gestureDetection;

        void Start()
        {
            gestureDetection = GestureDetection.Instance;
            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();

            if (gestureDetection == null)
            {
                Debug.LogError("GestureDetection instance not found. Please ensure that the GestureDetection prefab is in the scene.");
                return;
            }

            gestureDetection.headTransform = head;
            gestureDetection.leftHandTransform = handLeft;
            gestureDetection.rightHandTransform = handRight;
        }

        void OnSceneChange()
        {
            gestureDetection = GestureDetection.Instance;
            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();

            if (gestureDetection == null)
            {
                Debug.LogError("GestureDetection instance not found. Please ensure that the GestureDetection prefab is in the scene.");
                return;
            }

            gestureDetection.headTransform = head;
            gestureDetection.leftHandTransform = handLeft;
            gestureDetection.rightHandTransform = handRight;

            if (forceObjectsActive != null)
            {
                foreach (Transform t in forceObjectsActive)
                {
                    t.gameObject.SetActive(true);
                }
            }
        }

        void FixedUpdate()
        {
            if (head == null || handLeft == null || handRight == null)
            {
                return;
            }

            if (gestureDetection == null)
                gestureDetection = GestureDetection.Instance;

            if (gestureDetection == null)
                gestureDetection = FindObjectOfType<GestureDetection>();

            if (gestureDetection == null)
            {
                Debug.LogError("GestureDetection instance not found. Please ensure that the GestureDetection prefab is in the scene.");
                return;
            }

            if (gestureDetection.headTransform == null || gestureDetection.leftHandTransform == null || gestureDetection.rightHandTransform == null)
            {
                gestureDetection.headTransform = head;
                gestureDetection.leftHandTransform = handLeft;
                gestureDetection.rightHandTransform = handRight;

                if (forceObjectsActive != null)
                {
                    foreach (Transform t in forceObjectsActive)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }
        }

    }
}