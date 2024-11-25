using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Data;

namespace VRGestureDetection.Data
{
    /// <summary>
    /// Represents recordings of a specific gesture, containing multiple gesture data samples.
    /// </summary>
    [System.Serializable]
    public class GestureRecordings
    {
        /// <summary>
        /// The name of the gesture.
        /// </summary>
        public string gestureName;

        /// <summary>
        /// The name of the device
        /// </summary>
        public string deviceName = "unknown_device";

        /// <summary>
        /// A list of recordings, each containing gesture data samples.
        /// </summary>
        public List<GestureData> recordings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureRecordings"/> class with a specified gesture name.
        /// </summary>
        /// <param name="name">The name of the gesture.</param>
        public GestureRecordings(string name, string device)
        {
            gestureName = name;
            deviceName = device;
            recordings = new List<GestureData>();
        }
    }
}
