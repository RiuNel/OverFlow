using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using VRGestureDetection.Data;
using Regex = System.Text.RegularExpressions.Regex;

namespace VRGestureDetection.Core
{
    /// <summary>
    /// Represents a gesture event with a name and an associated UnityEvent that triggers when the gesture is detected.
    /// </summary>
    [System.Serializable]
    public class GestureEvent
    {
        public string gestureName;
        public UnityEvent onGestureDetected;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GestureEvent"/> class with a specified gesture name and UnityEvent.
        /// </summary>

        public GestureEvent(string gestureName, UnityEvent onGestureDetected)
        {
            this.gestureName = gestureName;
            this.onGestureDetected = onGestureDetected;
        }

        /// <summary>
        /// Initializes anew instance of the <see cref="GestureEvent"/> class.
        /// </summary>
        public GestureEvent()
        {
        }
    }

    /// <summary>
    /// Main class for VR gesture detection. Handles initialization, data processing, and gesture event invocation.
    /// </summary>
    public class GestureDetection : MonoBehaviour
    {
        public static GestureDetection Instance { get; private set; }

        private List<string> gestureNames;

        public bool keepThisInstance = false;
        private List<GestureEvent> gestureEvents = new List<GestureEvent>();
        [Tooltip("Transform of the player controller.")]
        public Transform playerControllerTransform;

        public string deviceName = "";

        [Tooltip("Transform of the head.")]
        public Transform headTransform;

        [Tooltip("Transform of the left hand.")]
        public Transform leftHandTransform;

        [Tooltip("Transform of the right hand.")]
        public Transform rightHandTransform;

        [Tooltip("Characteristics of the left hand controller.")]
        public InputDeviceCharacteristics controllerCharacteristicsLeft;

        [Tooltip("Characteristics of the right hand controller.")]
        public InputDeviceCharacteristics controllerCharacteristicsRight;

        [Tooltip("Characteristics of the head controller.")]
        public InputDeviceCharacteristics controllerCharacteristicsHead;
        private InputDevice leftHandDevice, rightHandDevice, headDevice;

        private const int MAX_RECORD_COUNT = 20;
        private const int COOLDOWN_FRAMES_DEFAULT = 10;
        private int cooldownFrames = COOLDOWN_FRAMES_DEFAULT;

        private GestureData currentTesting;

        private bool inferenceLoaded = false;

        [Tooltip("The neural network model for gesture detection.")]
        [SerializeField]
        private NNModel modelAsset;

        public bool enableDetection = true;
        private Model _runtimeModel;
        private IWorker _worker;
        private Texture2D gestureImage;
        private string currentGesture, lastGesture;
        private float lastProbability;

        /// <summary>
        /// Singleton pattern to ensure only one instance exists.
        /// </summary>
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                if (keepThisInstance)
                {
                    Destroy(Instance.gameObject);
                } else {
                    Destroy(gameObject);
                }
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// Subscribes to the deviceConnected event on enable.
        /// </summary>
        void OnEnable()
        {
            InputDevices.deviceConnected += OnDeviceConnected;
        }

        /// <summary>
        /// Unsubscribes from the deviceConnected event on disable.
        /// </summary>
        void OnDisable()
        {
            InputDevices.deviceConnected -= OnDeviceConnected;
        }

        /// <summary>
        /// Handles device connection and assigns the appropriate device references.
        /// </summary>
        /// <param name="device">The connected input device.</param>
        private void OnDeviceConnected(InputDevice device)
        {
            if (device.characteristics.HasFlag(controllerCharacteristicsLeft))
            {
                if ( device.name.Contains("Controller") )
                {
                    leftHandDevice = device;
                    DeriveDeviceName(device);
                } else {
                    Debug.Log("Non controller device initialized: " + device.name);
                }
            }
            else if (device.characteristics.HasFlag(controllerCharacteristicsRight))
            {
                if ( device.name.Contains("Controller") )
                {
                    rightHandDevice = device;
                    DeriveDeviceName(device);
                    Debug.Log("Right hand device initialized: " + device.name);
                } else {
                    Debug.Log("Non controller device initialized: " + device.name);
                }
            }
            else if (device.characteristics.HasFlag(controllerCharacteristicsHead))
            {
                headDevice = device;
                Debug.Log("Head device initialized: " + device.name);
            }
        }

        /// <summary>
        /// Derives the device name from the input device in sort of a silly way.
        /// </summary>
        /// <param name="device"></param>
        void DeriveDeviceName(InputDevice device)
        {
            if (deviceName != "")
            {
                return;
            }

            deviceName = device.name.ToLower();
            deviceName = deviceName.Replace(" ", "_");
            deviceName = deviceName.Replace("left", "");
            deviceName = deviceName.Replace("right", "");
            deviceName = deviceName.Replace("controller", "");
            deviceName = deviceName.Replace("-", "_");
            deviceName = deviceName.Replace("___", "_");
            deviceName = deviceName.Replace("__", "_");
        }

        /// <summary>
        /// Initializes gesture detection on start.
        /// </summary>
        void Start()
        {
            LoadGestureNames();
            currentTesting = new GestureData(MAX_RECORD_COUNT);
            InitializeConnectedDevices();
        }

        /// <summary>
        /// Initializes connected devices at the start.
        /// </summary>
        void InitializeConnectedDevices()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevices(devices);
            foreach (var device in devices)
            {
                OnDeviceConnected(device);
            }
        }

        /// <summary>
        /// Loads the gesture recognition model for inference.
        /// </summary>
        void LoadInference()
        {
            _runtimeModel = ModelLoader.Load(modelAsset);
            _worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, _runtimeModel);
            inferenceLoaded = true;
        }

        /// <summary>
        /// Performs fixed updates to process gestures and manage cooldowns.
        /// </summary>
        private void FixedUpdate()
        {
            if (!enableDetection)
            {
                return;
            }

            if (!inferenceLoaded)
            {
                LoadInference();
            }

            AddCurrentFrameData();

            if (currentTesting.leftHandData.Count == MAX_RECORD_COUNT && cooldownFrames <= 0)
            {
                ProcessCurrentGesture();
            }

            if (cooldownFrames > 0)
            {
                cooldownFrames--;
            }
            else
            {
                lastGesture = "";
            }
        }

        /// <summary>
        /// Adds the current frame's data to the gesture data buffer.
        /// </summary>
        private void AddCurrentFrameData()
        {
            if (leftHandTransform == null || rightHandTransform == null || headTransform == null)
            {
                return;
            }

            currentTesting.AddData(
                new TransformData(leftHandTransform, leftHandDevice, headTransform),
                new TransformData(rightHandTransform, rightHandDevice, headTransform),
                new TransformData(headTransform, headDevice, currentTesting.AverageHeadPosition())
            );
        }

        /// <summary>
        /// Processes the current gesture by generating an image and performing inference.
        /// </summary>
        private void ProcessCurrentGesture()
        {
            gestureImage = GestureToImage.GenerateImage(currentTesting, deviceName);

            if (gestureImage != null)
            {
                // Tensor를 using 블록으로 관리하여 리소스를 안전하게 해제
                using (Tensor tensorInput = ConvertTextureToTensor(gestureImage))
                {
                    _worker.Execute(tensorInput);

                    // Output Tensor를 using 블록으로 관리
                    using (Tensor output = _worker.PeekOutput())
                    {
                        ProcessOutput(output);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the output from the inference model and invokes gesture events if a new gesture is detected.
        /// </summary>
        /// <param name="output">The output tensor from the inference model.</param>
        private void ProcessOutput(Tensor output)
        {
            if (cooldownFrames > 0)
            {
                return;
            }

            var probabilities = output.ToReadOnlyArray();
            float highestProbability = 0.95f;
            currentGesture = "";

            for (int i = 0; i < probabilities.Length; i++)
            {
                var probability = probabilities[i];
                if (probability > highestProbability && !gestureNames[i].Contains("idle"))
                {
                    highestProbability = probability;
                    currentGesture = gestureNames[i];
                    lastProbability = highestProbability;
                }
            }

            if (!string.IsNullOrEmpty(currentGesture) && currentGesture != lastGesture)
            {
                lastGesture = currentGesture;
                InvokeGestureEvent(currentGesture);
                cooldownFrames = COOLDOWN_FRAMES_DEFAULT;
            }
        }

        /// <summary>
        /// Invokes the associated UnityEvent for the detected gesture.
        /// </summary>
        /// <param name="gestureName">The name of the detected gesture.</param>
        private void InvokeGestureEvent(string gestureName)
        {
            foreach (var gestureEvent in gestureEvents)
            {
                if (gestureEvent.gestureName == gestureName && gestureEvent.onGestureDetected != null)
                {
                    gestureEvent.onGestureDetected.Invoke();
                    break;
                }
            }
        }

        /// <summary>
        /// Converts a Texture2D to a Tensor for inference.
        /// </summary>
        /// <param name="texture">The texture to convert.</param>
        /// <returns>The converted Tensor.</returns>
        Tensor ConvertTextureToTensor(Texture2D texture)
        {
            float[] tensorData = new float[texture.height * texture.width * 4];

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    Color pixel = texture.GetPixel(x, y);
                    int index = (y * texture.width + x) * 4;
                    tensorData[index + 0] = pixel.r * 255;
                    tensorData[index + 1] = pixel.g * 255;
                    tensorData[index + 2] = pixel.b * 255;
                    tensorData[index + 3] = pixel.a * 255;
                }
            }

            return new Tensor(1, texture.height, texture.width, 4, tensorData);
        }

        /// <summary>
        /// Loads gesture names from a JSON resource file.
        /// </summary>
        private void LoadGestureNames()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Gestures");
            if (jsonFile != null)
            {
                string jsonData = jsonFile.text;
                GestureList loadedData = JsonUtility.FromJson<GestureList>(jsonData);
                this.gestureNames = loadedData.gestureNames;
            }
            else
            {
                Debug.LogError("File not found: Gestures.json");
                return;
            }
        }

        /// <summary>
        /// Clears all gesture events.
        /// </summary>
        public void ClearGestureEvents()
        {
            this.gestureEvents.Clear();
        }

        /// <summary>
        /// Adds a new gesture event to the list of gesture events.
        /// </summary>
        /// <param name="gestureEvent">The gesture event to add.</param>
        public void AddGestureEvent(GestureEvent gestureEvent)
        {
            this.gestureEvents.Add(gestureEvent);
        }
        
        /// <summary>
        /// Get the gestureEvents list.
        /// </summary>
        /// <returns>The gestureEvents list.</returns>
        public List<GestureEvent> GetGestureEvents()
        {
            return gestureEvents;
        }

        /// <summary>
        /// Gets the current gesture image.
        /// </summary>
        /// <returns>The current gesture image.</returns>
        public Texture2D GetCurrentGestureImage()
        {
            return gestureImage;
        }

        /// <summary>
        /// Gets the transform of the specified hand.
        /// </summary>
        /// <param name="side">The side of the hand ("left" or "right").</param>
        /// <returns>The transform of the specified hand.</returns>
        public Transform GetHandTransform(string side)
        {
            if (side == "left")
            {
                return leftHandTransform;
            }
            else if (side == "right")
            {
                return rightHandTransform;
            }
            return null;
        }

        /// <summary>
        /// Gets the input device of the specified hand.
        /// </summary>
        /// <param name="side">The side of the hand ("left" or "right").</param>
        /// <returns>The input device of the specified hand.</returns>
        public InputDevice GetHandDevice(string side)
        {
            if (side == "left")
            {
                return leftHandDevice;
            }
            else if (side == "right")
            {
                return rightHandDevice;
            }
            return rightHandDevice;
        }

        /// <summary>
        /// Gets the head input device.
        /// </summary>
        /// <returns>The head input device.</returns>
        public InputDevice GetHeadDevice()
        {
            return headDevice;
        }

        /// <summary>
        /// Gets the current gesture image.
        /// </summary>
        /// <returns>The current gesture image.</returns>
        public Texture2D GetGestureImage()
        {
            return gestureImage;
        }

        /// <summary>
        /// Gets the current gesture data being tested.
        /// </summary>
        /// <returns>The current gesture data.</returns>
        public GestureData GetCurrentTesting()
        {
            return currentTesting;
        }

        /// <summary>
        /// Gets the name of the current gesture.
        /// </summary>
        /// <returns>The name of the current gesture.</returns>
        public string GetCurrentGesture()
        {
            return currentGesture;
        }

        /// <summary>
        /// Gets the probability of the last detected gesture.
        /// </summary>
        /// <returns>The probability of the last detected gesture.</returns>
        public float GetLastProbability()
        {
            return lastProbability;
        }

        /// <summary>
        /// Set Enable Detection.
        /// </summary>
        public void SetEnableDetection(bool enable)
        {
            enableDetection = enable;
        }

        private void OnDestroy()
        {
            // IWorker 리소스를 명시적으로 해제
            if (_worker != null)
            {
                _worker.Dispose();
                _worker = null;
            }
        }
    }
}
