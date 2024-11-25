/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class GestureCharacterController : MonoBehaviour
    {
        public GestureDetection gestureDetection;
 
        public CharacterController characterController;
        public AudioSource audioSource;
        public AudioClip runningSound, breastStrokeSound, underwaterSwimSound;

        public bool _running = false, _grounded = true, _inWater = false, _diving = false;
        private float _runStopTime = 0f, _runStopDelay = 0.3f;
        public float _waterSurfaceLevel = -2f;

        private Vector3 currentMovementDirection;
        public float currentSpeed = 0f;
        public float targetSpeed = 0f;
        private float heldSpeed = 0f;
        private float holdSpeedTime = 0.6f;
        private bool _accelerating = false;

        private Color defaultFogColor;
        private Color underwaterFogColor = new Color(0.0f, 0.4f, 0.7f, 1.0f);
        private float defaultFogDensity;
        private float underwaterFogDensity = 0.1f;

        void Start()
        {
            gestureDetection ??= GestureDetection.Instance;
            LoadGestures();

            defaultFogColor = RenderSettings.fogColor;
            defaultFogDensity = RenderSettings.fogDensity;
        }

        void LoadGestures()
        {
            AddGestureEvent("breast stroke", BreastStroke);
            AddGestureEvent("underwater swim", UnderwaterPush);
//            AddGestureEvent("clap", UnderwaterPull);
        }

        void AddGestureEvent(string gestureName, UnityEngine.Events.UnityAction action)
        {
            var gestureEvent = new GestureEvent { gestureName = gestureName };
            gestureEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            gestureEvent.onGestureDetected.AddListener(action);
            gestureDetection.AddGestureEvent(gestureEvent);
        }

        void Update()
        {
            CheckGrounded();
            HandleRunningGesture();
            HandleMovement();
            characterController.Move(currentMovementDirection * Time.deltaTime * currentSpeed);

            UpdateFogSettings();

            
        }

        void HandleRunningGesture()
        {
            string detectedGesture = gestureDetection.GetCurrentGesture();
            if (detectedGesture == "running" && _grounded && !_inWater)
            {
                _runStopTime = 0f;
                _running = true;
            }
            else
            {
                _runStopTime += Time.deltaTime;
                if (_runStopTime > _runStopDelay) _running = false;
            }
            Debug.Log(detectedGesture);
        }

        void HandleMovement()
        {
            if (_grounded && !_inWater)
            {
                HandleGroundedMovement();
            }
            else if (_grounded && _inWater)
            {
                HandleUnderwaterGroundedMovement();
            }
            else
            {
                HandleAirborneMovement();
            }
        }

        void HandleGroundedMovement()
        {
            _diving = false;
            currentMovementDirection.y = 0;

            if (_running)
            {
                MoveCharacter(Camera.main.transform.forward, 5f, runningSound, true);
            }
            else
            {
                SlowDownCharacter();
            }
        }

        void HandleUnderwaterGroundedMovement()
        {
            if (_running)
            {
                MoveCharacter(Camera.main.transform.forward, 1f, runningSound, true);
            }
            else
            {
                SlowDownCharacter();
            }

        }

        void HandleAirborneMovement()
        {
            if (!_inWater)
            {
                currentMovementDirection.y -= Time.deltaTime * 9.81f;
            }
            else
            {
                HandleWaterMovement();
            }
        }

        void HandleWaterMovement()
        {
            if (_diving)
            {
                currentMovementDirection = Camera.main.transform.forward;
                if (Camera.main.transform.position.y > _waterSurfaceLevel + 1.1f) _diving = false;
            }
            else
            {
                currentMovementDirection = Camera.main.transform.forward;
                currentMovementDirection.y = 0;
            }

            SlowDownCharacter();
        }

        void MoveCharacter(Vector3 direction, float speed, AudioClip sound, bool loop)
        {
            currentMovementDirection = direction;
            currentMovementDirection.y = 0;
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 1f);
            PlayAudio(sound, loop);
        }

        void SlowDownCharacter()
        {
            if (_accelerating)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 1f);
                if (Mathf.Abs(currentSpeed - targetSpeed) < 0.1f)
                {
                    currentSpeed = targetSpeed;
                    heldSpeed += Time.deltaTime;

                    if (heldSpeed >= holdSpeedTime)
                    {
                        _accelerating = false;
                    }
                }
            }
            else
            {
                currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 5f);
            }
            if (audioSource.clip == runningSound) audioSource.Stop();
        }

        void PlayAudio(AudioClip clip, bool loop)
        {
            audioSource.loop = loop;
            if (audioSource.clip != clip)
            {
                audioSource.clip = clip;
            }
            if (!audioSource.isPlaying) audioSource.Play();
        }

        void CheckGrounded()
        {
            _grounded = Physics.Raycast(characterController.transform.position, Vector3.down, out RaycastHit hit, 0.1f) && hit.collider.CompareTag("Ground");
            _inWater = characterController.transform.position.y < _waterSurfaceLevel;
        }

        void UpdateFogSettings()
        {
            if (Camera.main.transform.position.y < _waterSurfaceLevel + 1f)
            {
                RenderSettings.fogColor = underwaterFogColor;
                RenderSettings.fogDensity = underwaterFogDensity;
            }
            else
            {
                RenderSettings.fogColor = defaultFogColor;
                RenderSettings.fogDensity = defaultFogDensity;
            }
        }

        public void SetInWater(bool inWater, float waterSurfaceY)
        {
            _inWater = inWater;
            _waterSurfaceLevel = waterSurfaceY;
        }

        void BreastStroke()
        {
            if (_inWater && !_diving)
            {
                targetSpeed = 4f;
                heldSpeed = 0f;
                _accelerating = true;
                PlayAudio(breastStrokeSound, false);
            }
        }

        void UnderwaterPush()
        {
            if (_inWater)
            {
                if (!_diving)
                {
                    // push the player down 0.5 under the water surface
                    characterController.Move(Vector3.down * 0.5f);
                }
                _diving = true;
                targetSpeed = 4f;
                heldSpeed = 0f;
                _accelerating = true;
                PlayAudio(underwaterSwimSound, false);
            }
        }

        void UnderwaterPull()
        {
            if (_inWater)
            {
                _diving = true;
                targetSpeed = -4f;
                heldSpeed = 0f;
                _accelerating = true;
                PlayAudio(underwaterSwimSound, false);
            }
        }
    }
}
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;


//안전하게 이동: uppercut right hand
//우로/좌로 lasso right/left
//정지: pulldown right hand
//긴급정지: windmill arms



namespace VRGestureDetection.Sample
{
    public class GestureCharacterController : MonoBehaviour
    {
        public GestureDetection gestureDetection; //제스쳐를 감지하고 넣을 변수

        public CharacterController characterController;
        public AudioSource audioSource;
        public AudioClip runningSound, breastStrokeSound, underwaterSwimSound;

        private Vector3 currentMovementDirection;
        public float currentSpeed = 0f;
        public float targetSpeed = 0f;
        private float heldSpeed = 0f;
        private float holdSpeedTime = 0.6f;
        private bool _accelerating = false;

        //트럭 이동 여부 변수....................
        
        // reverse move
        public bool truck_move = false;

        // left move
        public bool truck_move_left = false;
        public bool truck_stop = false;

        public bool truck_move_right = false;
        //......................................

        public string testGesture;

        void Start()
        {
            gestureDetection ??= GestureDetection.Instance;
            LoadGestures();
        }

        void LoadGestures()
        {

            //reverse move
            AddGestureEvent("two hand throw overhand", TruckMoveGesture);
            AddGestureEvent("orb hands rotate", TruckMoveGesture);
            AddGestureEvent("right hand circle", TruckMoveGesture);

            //left move
            AddGestureEvent("lasso left", TruckMoveGesture);
            AddGestureEvent("reeling in left hand", TruckMoveGesture);
            AddGestureEvent("pull down left hand", TruckMoveGesture);

            //right move
            AddGestureEvent("pull down right hand", TruckMoveGesture); 
            AddGestureEvent("upper cut right hand", TruckMoveGesture); // 좌로 이동
            AddGestureEvent("extended arm circles", TruckMoveGesture); // 우로 이동
        }

        void AddGestureEvent(string gestureName, UnityEngine.Events.UnityAction action)
        {
            var gestureEvent = new GestureEvent { gestureName = gestureName };
            gestureEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();
            gestureEvent.onGestureDetected.AddListener(action);
            gestureDetection.AddGestureEvent(gestureEvent);
        }

        void Update()
        {
            /*CheckGrounded(); 뭔지 잘 모르겠음
            HandleRunningGesture(); 
            HandleMovement();
            UpdateFogSettings();*/
            characterController.Move(currentMovementDirection * Time.deltaTime * currentSpeed);
            TruckMoveGesture();
            Debug.Log(gestureDetection.GetCurrentGesture());
            testGesture = gestureDetection.GetCurrentGesture();      
        }
        
        void TruckMoveGesture()
        {
            string detectedGesture = gestureDetection.GetCurrentGesture();




            if (detectedGesture == "two hand throw overhand" || detectedGesture == "orb hands rotate" || detectedGesture == "right hand circle")
            {
                truck_move = true; //안전하게 이동
            }
            else 
            {
                //truck_move = false;
            }

            if (detectedGesture == "lasso left" || detectedGesture == "reeling in left hand" || detectedGesture == "pull down left hand")
            {
                truck_move_left = true; //좌측 이동
            } 
            else
            {
                //truck_move_left = false;
            }

            if (detectedGesture == "pull down right hand" || detectedGesture == "upper cut right hand" || detectedGesture == "extended arm circles")
            {
                truck_move_right = true; //우측 이동
            }
            else
            {
                //truck_move_right = false;
            }


            if (detectedGesture == "windmill arms")
            {
                truck_stop = true; //정지
            }

            Debug.Log(detectedGesture);
        }
        public void TruckIdleReset()
        {
            truck_move = false;
            truck_move_left = false; 
            truck_move_right = false;
            truck_stop = false;
        }
        
    }
}

