using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;


//�����ϰ� �̵�: uppercut right hand
//���/�·� lasso right/left
//����: pulldown right hand
//�������: windmill arms



namespace VRGestureDetection.Sample
{
    public class GestureCharacterController : MonoBehaviour
    {
        public GestureDetection gestureDetection; //�����ĸ� �����ϰ� ���� ����

        public CharacterController characterController;
        public AudioSource audioSource;
        public AudioClip runningSound, breastStrokeSound, underwaterSwimSound;
        public GameObject levels;

        private Vector3 currentMovementDirection;
        public float currentSpeed = 0f;
        public float targetSpeed = 0f;
        //private float heldSpeed = 0f;
        //private float holdSpeedTime = 0.6f;
        //private bool _accelerating = false;

        //Ʈ�� �̵� ���� ����....................

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

            //left move
            AddGestureEvent("lasso left", TruckMoveGesture);
            AddGestureEvent("pull down left hand", TruckMoveGesture);

            //right move
            AddGestureEvent("pull down right hand", TruckMoveGesture);
            AddGestureEvent("upper cut right hand", TruckMoveGesture); // �·� �̵�

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
            /*CheckGrounded(); ���� �� �𸣰���
            HandleRunningGesture(); 
            HandleMovement();
            UpdateFogSettings();*/
            if (!levels.GetComponent<NarrationControl>().isNarrationPlaying)
            {
                characterController.Move(currentMovementDirection * Time.deltaTime * currentSpeed);
                testGesture = gestureDetection.GetCurrentGesture();
                TruckMoveGesture();
            }

            
            Debug.Log(gestureDetection.GetCurrentGesture());
            
        }

        void TruckMoveGesture()
        {
            string detectedGesture = gestureDetection.GetCurrentGesture();




            if (detectedGesture == "two hand throw overhand")
            {
                truck_move = true; //�����ϰ� �̵�
            }
            else
            {
                //truck_move = false;
            }

            if (detectedGesture == "lasso left" || detectedGesture == "pull down left hand")
            {
                truck_move_left = true; //���� �̵�
            }
            else
            {
                //truck_move_left = false;
            }

            if (detectedGesture == "pull down right hand" || detectedGesture == "upper cut right hand")
            {
                truck_move_right = true; //���� �̵�
            }
            else
            {
                //truck_move_right = false;
            }


            if (detectedGesture == "windmill arms")
            {
                truck_stop = true; //����
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