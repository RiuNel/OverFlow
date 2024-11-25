using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRGestureDetection.Utils
{
	[System.Serializable]
	public class SerializableVector3
	{
		[SerializeField] public float x;
		[SerializeField] public float y;
		[SerializeField] public float z;

		// Constructor
		public SerializableVector3(float rX, float rY, float rZ)
		{
			x = rX;
			y = rY;
			z = rZ;
		}

		// Returns a string representation of the object
		public override string ToString()
		{
			return string.Format("[{0}, {1}, {2}]", x, y, z);
		}

		// Automatic conversion from SerializableVector3 to Vector3
		public static implicit operator Vector3(SerializableVector3 rValue)
		{
			return new Vector3(rValue.x, rValue.y, rValue.z);
		}

		// Automatic conversion from Vector3 to SerializableVector3
		public static implicit operator SerializableVector3(Vector3 rValue)
		{
			return new SerializableVector3(rValue.x, rValue.y, rValue.z);
		}

		public SerializableVector3 MakeCopy()
		{
			return new SerializableVector3(this.x, this.y, this.z);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(this.x, this.y, this.z);
		}

		public Vector2 ToVector2()
		{
			return new Vector2(this.x, this.y);
		}
	}
}