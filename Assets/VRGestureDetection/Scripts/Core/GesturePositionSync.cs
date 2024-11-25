using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using VRGestureDetection.Core;

namespace VRGestureDetection.Core
{
    /// <summary>
    /// Synchronizes the positions of the head and hands with the gesture detection system.
    /// References the devices used by the gesture detection system and updates the positions of the head and hands accordingly.
    /// This is helpful when dealing with a complex XR setup.
    /// </summary>
    public class GesturePositionSync : MonoBehaviour
    {
        private GestureDetection gestureDetection;
        public Transform headTransform, leftHandTransform, rightHandTransform;

        /// <summary>
        /// Updates the positions of the head and hands at the end of each frame to ensure synchronization with the OpenXR system.
        /// </summary>
        void LateUpdate()
        {
            if (gestureDetection == null)
                gestureDetection = GestureDetection.Instance;

            if (gestureDetection == null)
            {
                return;
            }

            // Sync the position of the head and hands with the Open XR system
            headTransform.position = gestureDetection.GetHeadDevice().TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headPosition) ? headPosition : headTransform.position;
            headTransform.rotation = gestureDetection.GetHeadDevice().TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headRotation) ? headRotation : headTransform.rotation;

            leftHandTransform.position = gestureDetection.GetHandDevice("left").TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftHandPosition) ? leftHandPosition : leftHandTransform.position;
            leftHandTransform.rotation = gestureDetection.GetHandDevice("left").TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion leftHandRotation) ? leftHandRotation : leftHandTransform.rotation;

            rightHandTransform.position = gestureDetection.GetHandDevice("right").TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightHandPosition) ? rightHandPosition : rightHandTransform.position;
            rightHandTransform.rotation = gestureDetection.GetHandDevice("right").TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rightHandRotation) ? rightHandRotation : rightHandTransform.rotation;

            // Normalize directions if needed
            headTransform.forward = NormalizeDirection(headTransform.forward);
            leftHandTransform.forward = NormalizeDirection(leftHandTransform.forward);
            rightHandTransform.forward = NormalizeDirection(rightHandTransform.forward);
        }
        void NormalizeAxes()
        {
            // Assuming you have references to the head and hand transforms
            Vector3 headForward = headTransform.forward;
            Vector3 leftHandForward = leftHandTransform.forward;
            Vector3 rightHandForward = rightHandTransform.forward;

            // Ensure the forward directions are consistent
            headForward = NormalizeDirection(headForward);
            leftHandForward = NormalizeDirection(leftHandForward);
            rightHandForward = NormalizeDirection(rightHandForward);

            // Apply the normalized directions back to the transforms
            headTransform.forward = headForward;
            leftHandTransform.forward = leftHandForward;
            rightHandTransform.forward = rightHandForward;
        }

        Vector3 NormalizeDirection(Vector3 direction)
        {
            // Normalize the direction vector
            return direction.normalized;
        }

    }
}