using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class WaterAnimator : MonoBehaviour
    {
        public Material waterMaterial;  // The material to be animated
        public float scrollSpeedX = 0.1f;  // Speed of the offset animation in the X direction
        public float scrollSpeedY = 0.1f;  // Speed of the offset animation in the Y direction

        private Vector2 offset;

        void Start()
        {
            if (waterMaterial == null)
            {
                Debug.LogError("Water material is not assigned.");
            }
        }

        void Update()
        {
            if (waterMaterial != null)
            {
                // Calculate the new offset based on the scroll speed and time
                offset.x += scrollSpeedX * Time.deltaTime;
                offset.y += scrollSpeedY * Time.deltaTime;

                // Retrieve the current tiling and offset
                Vector4 baseMapST = waterMaterial.GetVector("_BaseMap_ST");

                // Update the offset values
                baseMapST.z = offset.x;
                baseMapST.w = offset.y;

                // Apply the updated offset to the material
                waterMaterial.SetVector("_BaseMap_ST", baseMapST);
            }
        }
    }
}