using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Data;

namespace VRGestureDetection.Core
{
    public class GestureToImage
    {
        public const int ImageSize = 27; 
        public const int MaxFrameCount = 20;

        public static float[] minFloatValues = { -1.5f, -0.5f, -1.5f, 0f, 0f, 0f, -8f, -8f, -8f, -26f, -26f, -26f };
        public static float[] maxFloatValues = { 1.5f, 0.5f, 1.5f, 360f, 360f, 360f, 8f, 8f, 8f, 26f, 26f, 26f };

        /// <summary>
        /// Generates an image from the specified gesture data.
        /// </summary>
        /// <param name="gestureData"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static Texture2D GenerateImage(GestureData gestureData, string deviceName)
        {
            Texture2D texture = new Texture2D(ImageSize, ImageSize);
            ClearTexture(texture, Color.black);

            Color[] pixels = texture.GetPixels();
            
            // Encode additional data in the top 7 rows
            EncodeSummaryData(pixels, gestureData);

            int frameCount = Mathf.Min(MaxFrameCount, gestureData.leftHandData.Count, gestureData.rightHandData.Count, gestureData.headData.Count);
            
            for (int frame = 0; frame < frameCount; frame++)
            {
                int rowStartIndex = (frame + 7) * ImageSize; // Start after the top 7 rows

                for (int objectIndex = 0; objectIndex < 2; objectIndex++)
                {
                    for (int vectorIndex = 0; vectorIndex < 4; vectorIndex++)
                    {
                        Vector3 currentValue = GetValue(gestureData, frame, objectIndex, vectorIndex);
                        int basePixelIndex = rowStartIndex + objectIndex * 12 + vectorIndex * 3;

                        Color encodedColor = EncodeColorFromValue(currentValue, vectorIndex);

                        // Assign color channels separately to ensure clear differentiation
                        pixels[basePixelIndex] = new Color(encodedColor.r, 0, 0);
                        pixels[basePixelIndex + 1] = new Color(0, encodedColor.g, 0);
                        pixels[basePixelIndex + 2] = new Color(0, 0, encodedColor.b);
                    }
                }

                // Compress head data into a single stream
                Vector3 headValue = GetValue(gestureData, frame, 2, 0);
                Color headColor = EncodeColorFromValue(headValue, 0);
                pixels[rowStartIndex + 24] = new Color(headColor.r, headColor.g, headColor.b);
            }

            // bottom right 2x2 pixels should be color coded based on deviceName, blue for 'index', green for 'quest3' yellow for 'quest2' and purple for other
            Color deviceColor = GetDeviceColor(deviceName);

            int bottomRightIndex = (ImageSize - 1) * ImageSize + (ImageSize - 1);
            int bottomLeftIndex = bottomRightIndex - 1;
            int secondLastRowRightIndex = bottomRightIndex - ImageSize;
            int secondLastRowLeftIndex = secondLastRowRightIndex - 1;

            pixels[bottomRightIndex] = deviceColor;
            pixels[bottomLeftIndex] = deviceColor;
            pixels[secondLastRowRightIndex] = deviceColor;
            pixels[secondLastRowLeftIndex] = deviceColor;

            texture.SetPixels(pixels);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();

            return texture;
        }

        /// <summary>
        /// Generates a color based on the specified device name.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        public static Color GetDeviceColor(string deviceName)
        {
            int hash = deviceName.GetHashCode();
            float r = (hash & 0xFF0000) >> 16;
            float g = (hash & 0x00FF00) >> 8;
            float b = hash & 0x0000FF;

            return new Color(r / 255f, g / 255f, b / 255f);
        }

        /// <summary>
        /// Clears the texture with the specified color.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        private static void ClearTexture(Texture2D texture, Color color)
        {
            Color[] clearPixels = new Color[ImageSize * ImageSize];
            for (int i = 0; i < clearPixels.Length; i++)
            {
                clearPixels[i] = color;
            }
            texture.SetPixels(clearPixels);
        }

        /// <summary>
        /// Encodes summary data into the top 7 rows of the image.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="gestureData"></param>
        private static void EncodeSummaryData(Color[] pixels, GestureData gestureData)
        {
            // Example: Encode the average position and velocity of the left and right hand over all frames
            Vector3 avgLeftHandPosition = Vector3.zero;
            Vector3 avgRightHandPosition = Vector3.zero;
            Vector3 avgLeftHandVelocity = Vector3.zero;
            Vector3 avgRightHandVelocity = Vector3.zero;

            int frameCount = Mathf.Min(MaxFrameCount, gestureData.leftHandData.Count, gestureData.rightHandData.Count);
            
            for (int frame = 0; frame < frameCount; frame++)
            {
                avgLeftHandPosition += gestureData.leftHandData[frame].position.ToVector3();
                avgRightHandPosition += gestureData.rightHandData[frame].position.ToVector3();
                avgLeftHandVelocity += gestureData.leftHandData[frame].velocity.ToVector3();
                avgRightHandVelocity += gestureData.rightHandData[frame].velocity.ToVector3();
            }

            avgLeftHandPosition /= frameCount;
            avgRightHandPosition /= frameCount;
            avgLeftHandVelocity /= frameCount;
            avgRightHandVelocity /= frameCount;

            // Encode the averages into the top 7 rows (27 pixels per row)
            EncodeVector3IntoPixels(pixels, 0, avgLeftHandPosition);
            EncodeVector3IntoPixels(pixels, 3, avgRightHandPosition);
            EncodeVector3IntoPixels(pixels, 6, avgLeftHandVelocity);
            EncodeVector3IntoPixels(pixels, 9, avgRightHandVelocity);
        }

        /// <summary>
        /// Encodes a Vector3 value into the specified pixels array.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        private static void EncodeVector3IntoPixels(Color[] pixels, int startIndex, Vector3 value)
        {
            float r = NormalizeValue(value.x, 0);
            float g = NormalizeValue(value.y, 1);
            float b = NormalizeValue(value.z, 2);
            Color color = new Color(r, g, b);

            pixels[startIndex] = color;
        }

        /// <summary>
        /// Gets the value of the specified vector from the gesture data.
        /// </summary>
        /// <param name="gestureData"></param>
        /// <param name="frame"></param>
        /// <param name="objectIndex"></param>
        /// <param name="vectorIndex"></param>
        private static Vector3 GetValue(GestureData gestureData, int frame, int objectIndex, int vectorIndex)
        {
            TransformData transformData = new TransformData();
            switch (objectIndex)
            {
                case 0:
                    transformData = gestureData.leftHandData[frame];
                    break;
                case 1:
                    transformData = gestureData.rightHandData[frame];
                    break;
                case 2:
                    transformData = gestureData.headData[frame];
                    break;
            }

            Vector3 value = Vector3.zero;
            switch (vectorIndex)
            {
                case 0:
                    value = transformData.position.ToVector3();
                    break;
                case 1:
                    value = transformData.rotation.ToVector3();
                    break;
                case 2:
                    value = transformData.velocity.ToVector3();
                    break;
                case 3:
                    value = transformData.angularVelocity.ToVector3();
                    break;
            }
            return value;
        }

        /// <summary>
        /// Encodes a Vector3 value into a color.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="vectorIndex"></param>
        /// <returns></returns>
        private static Color EncodeColorFromValue(Vector3 value, int vectorIndex)
        {
            float r = NormalizeValue(value.x, vectorIndex * 3);
            float g = NormalizeValue(value.y, vectorIndex * 3 + 1);
            float b = NormalizeValue(value.z, vectorIndex * 3 + 2);
            return new Color(r, g, b);
        }

        /// <summary>
        /// Normalizes the specified value based on the min and max values of the specified column index.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnIndex"></param>
        private static float NormalizeValue(float value, int columnIndex)
        {
            return (value - minFloatValues[columnIndex]) / (maxFloatValues[columnIndex] - minFloatValues[columnIndex]);
        }
    }
}
