using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGestureDetection.Utils
{
    [System.Serializable]
    public class SerializableQuaternion
    {
        [SerializeField] public float x;
        [SerializeField] public float y;
        [SerializeField] public float z;
        [SerializeField] public float w;

        // Constructor
        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }

        // Returns a string representation of the object
        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }

        // Automatic conversion from SerializableQuaternion to Quaternion
        public static implicit operator Quaternion(SerializableQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        // Automatic conversion from Quaternion to SerializableQuaternion
        public static implicit operator SerializableQuaternion(Quaternion rValue)
        {
            return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }

        public SerializableQuaternion MakeCopy()
        {
            return new SerializableQuaternion(this.x, this.y, this.z, this.w);
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion(this.x, this.y, this.z, this.w);
        }
    }
}