using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRGestureDetection.Data
{
    /// <summary>
    /// Represents the data for a gesture, including the transform data for the left hand, right hand, and head.
    /// </summary>
    [System.Serializable]
    public class GestureData
    {
        private int FixedLength = 40;
        
        /// <summary>
        /// List of transform data for the left hand.
        /// </summary>
        public List<TransformData> leftHandData;
        
        /// <summary>
        /// List of transform data for the right hand.
        /// </summary>
        public List<TransformData> rightHandData;
        
        /// <summary>
        /// List of transform data for the head.
        /// </summary>
        public List<TransformData> headData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureData"/> class with a specified maximum frame count.
        /// </summary>
        /// <param name="maxFrames">The maximum number of frames to store.</param>
        public GestureData(int maxFrames)
        {
            this.FixedLength = maxFrames;
            this.leftHandData = new List<TransformData>();
            this.rightHandData = new List<TransformData>();
            this.headData = new List<TransformData>();
        }

        /// <summary>
        /// Adds transform data for the current frame.
        /// </summary>
        /// <param name="leftHand">The transform data for the left hand.</param>
        /// <param name="rightHand">The transform data for the right hand.</param>
        /// <param name="head">The transform data for the head.</param>
        public void AddData(TransformData leftHand, TransformData rightHand, TransformData head)
        {
            this.leftHandData.Add(leftHand);
            this.rightHandData.Add(rightHand);
            this.headData.Add(head);

            RemoveOldestDataIfNeeded(this.leftHandData);
            RemoveOldestDataIfNeeded(this.rightHandData);
            RemoveOldestDataIfNeeded(this.headData);
        }

        /// <summary>
        /// Removes the oldest transform data if the list exceeds the fixed length.
        /// </summary>
        /// <param name="dataList">The list of transform data.</param>
        private void RemoveOldestDataIfNeeded(List<TransformData> dataList)
        {
            if (dataList.Count > FixedLength)
            {
                dataList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Converts the transform data at a specified index to a float array.
        /// </summary>
        /// <param name="i">The index of the transform data.</param>
        /// <returns>A float array representing the transform data.</returns>
        public float[] ToFloatArray(int i)
        {
            if (this.leftHandData.Count <= i || this.rightHandData.Count <= i || this.headData.Count <= i)
            {
                return new float[0]; // Or throw an exception
            }

            return this.leftHandData[i].ToFloatArray()
                .Concat(this.rightHandData[i].ToFloatArray())
                .Concat(this.headData[i].ToFloatArray())
                .ToArray();
        }

        /// <summary>
        /// Clears all transform data.
        /// </summary>
        public void Clear()
        {
            ClearDataList(this.leftHandData);
            ClearDataList(this.rightHandData);
            ClearDataList(this.headData);
        }

        /// <summary>
        /// Clears a specified list of transform data.
        /// </summary>
        /// <param name="dataList">The list of transform data to clear.</param>
        private void ClearDataList(List<TransformData> dataList)
        {
            if (dataList.Count > 0)
            {
                dataList.Clear();
            }
        }

        /// <summary>
        /// Calculates the average position of the head from the transform data.
        /// </summary>
        /// <returns>The average position of the head.</returns>
        public Vector3 AverageHeadPosition()
        {
            if (this.headData.Count == 0)
                return Vector3.zero;

            Vector3 sum = Vector3.zero;
            foreach (TransformData data in this.headData)
            {
                sum += data.position.ToVector3();
            }
            return sum / this.headData.Count;
        }
    }

    /// <summary>
    /// Represents a list of gesture names.
    /// </summary>
    public class GestureList
    {
        /// <summary>
        /// List of gesture names.
        /// </summary>
        public List<string> gestureNames;
    }
}
