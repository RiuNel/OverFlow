using UnityEngine;
using UnityEngine.XR;
using VRGestureDetection.Utils;

namespace VRGestureDetection.Data
{
    /// <summary>
    /// Represents the transform data for a specific device, including position, rotation, velocity, and angular velocity.
    /// </summary>
    [System.Serializable]
    public struct TransformData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
        public SerializableVector3 velocity;
        public SerializableVector3 angularVelocity;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformData"/> struct with the specified transform, device, and head transform.
        /// </summary>
        /// <param name="transform">The transform of the device.</param>
        /// <param name="device">The input device.</param>
        /// <param name="headTransform">The transform of the head.</param>
        public TransformData(Transform transform, InputDevice device, Transform headTransform)
        {
            Vector3 localPosition = transform.position - headTransform.position;
            Quaternion inverseHeadRotation = Quaternion.Inverse(headTransform.rotation);
            position = inverseHeadRotation * localPosition;

            rotation = NormalizeRotation(transform.localRotation.eulerAngles);

            velocity = new SerializableVector3(0, 0, 0);
            angularVelocity = new SerializableVector3(0, 0, 0);

            Vector3 deviceVelocity;
            Vector3 deviceAngularVel;
            if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity))
            {
                velocity = inverseHeadRotation * deviceVelocity;
            }
            if (device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVel))
            {
                angularVelocity = inverseHeadRotation * deviceAngularVel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformData"/> struct with the specified transform, device, and average head position.
        /// </summary>
        /// <param name="transform">The transform of the device.</param>
        /// <param name="device">The input device.</param>
        /// <param name="averageHeadPosition">The average position of the head.</param>
        public TransformData(Transform transform, InputDevice device, Vector3 averageHeadPosition)
        {
            position = transform.position - averageHeadPosition + new Vector3(0f, averageHeadPosition.y, 0f);
            rotation = NormalizeRotation(transform.rotation.eulerAngles);

            velocity = new SerializableVector3(0, 0, 0);
            angularVelocity = new SerializableVector3(0, 0, 0);

            Vector3 deviceVelocity;
            Vector3 deviceAngularVel;
            if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity))
            {
                Quaternion inverseHeadRotation = Quaternion.Inverse(transform.rotation);
                velocity = inverseHeadRotation * deviceVelocity;
            }
            if (device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out deviceAngularVel))
            {
                Quaternion inverseHeadRotation = Quaternion.Inverse(transform.rotation);
                angularVelocity = inverseHeadRotation * deviceAngularVel;
            }
        }

        /// <summary>
        /// Converts the transform data to a float array.
        /// </summary>
        /// <returns>A float array representing the transform data.</returns>
        public float[] ToFloatArray()
        {
            return new float[]
            {
                position.x, position.y, position.z,
                rotation.x, rotation.y, rotation.z,
                velocity.x, velocity.y, velocity.z,
                angularVelocity.x, angularVelocity.y, angularVelocity.z
            };
        }

        /// <summary>
        /// Normalizes the rotation values to a specific range.
        /// </summary>
        /// <param name="rotation">The rotation values.</param>
        /// <returns>The normalized rotation values.</returns>
        private static SerializableVector3 NormalizeRotation(Vector3 rotation)
        {
            rotation.x = NormalizeAngle(rotation.x - 90f);
            rotation.y = NormalizeAngle(rotation.y - 90f);
            rotation.z = NormalizeAngle(rotation.z - 90f);
            return rotation;
        }

        /// <summary>
        /// Normalizes an angle to the range [0, 360).
        /// </summary>
        /// <param name="angle">The angle to normalize.</param>
        /// <returns>The normalized angle.</returns>
        private static float NormalizeAngle(float angle)
        {
            if (angle < 0f)
            {
                angle += 360f;
            }
            return angle;
        }
    }
}
