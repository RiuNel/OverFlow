using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Input;
using VRGestureDetection.Core;

namespace VRGestureDetection.Sample
{
    public class GestureCombatSimulator : MonoBehaviour
    {
        public GestureDetection gestureDetection;
        public PulseEffect groundPulsePrefab, pulseEffect;
        public GestureBow bowObjectPrebab;
        public GameObject javelinPrefab;
        public GestureFrisbee frisbeePrefab;
        public bool bowDrawn = false;
        public Smashable[] smashables;
        GameObject[] boxes;

        Vector3[] boxPositions;
        Quaternion[] boxRotations;

        float moveCooldown = 0f;
        float moveCooldownTime = 0.4f;

        void Start()
        {
            if (gestureDetection == null)
                gestureDetection = GestureDetection.Instance;

            boxes = GameObject.FindGameObjectsWithTag("Box");

            boxPositions = new Vector3[boxes.Length];
            boxRotations = new Quaternion[boxes.Length];

            for (int i = 0; i < boxes.Length; i++)
            {
                boxPositions[i] = boxes[i].transform.position;
                boxRotations[i] = boxes[i].transform.rotation;
            }

            LoadGestures();

        }
        
        void Update()
        {
            if (moveCooldown > 0f)
            {
                moveCooldown -= Time.deltaTime;
            }
        }

        void LoadGestures()
        {
            gestureDetection.ClearGestureEvents();

            GestureEvent drawBowRight = new GestureEvent();
            drawBowRight.gestureName = "draw bow right";
            drawBowRight.onGestureDetected = new UnityEngine.Events.UnityEvent();
            drawBowRight.onGestureDetected.AddListener(() => 
            {
                DrawBow("right");
            });
            gestureDetection.AddGestureEvent(drawBowRight);

            GestureEvent drawBowLeft = new GestureEvent();
            drawBowLeft.gestureName = "draw bow left";
            drawBowLeft.onGestureDetected = new UnityEngine.Events.UnityEvent();
            drawBowLeft.onGestureDetected.AddListener(() => 
            {
                DrawBow("left");
            });
            gestureDetection.AddGestureEvent(drawBowLeft);

/*
            GestureEvent clap = new GestureEvent();
            clap.gestureName = "clap";
            clap.onGestureDetected = new UnityEngine.Events.UnityEvent();
            clap.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f && !bowDrawn)
                {
                    PulseEffect pulse = Instantiate(groundPulsePrefab);
                    pulse.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    pulse.transform.position = new Vector3(pulse.transform.position.x, -1.2f, pulse.transform.position.z);
                    pulse.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(clap);
*/

            GestureEvent throwFrisbeeRight = new GestureEvent();
            throwFrisbeeRight.gestureName = "frisbee throw right";
            throwFrisbeeRight.onGestureDetected = new UnityEngine.Events.UnityEvent();
            throwFrisbeeRight.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f && !bowDrawn)
                {
                    GestureFrisbee frisbee = Instantiate(frisbeePrefab);
                    frisbee.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    frisbee.transform.rotation = Camera.main.transform.rotation;

                    Rigidbody rb = frisbee.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * 20f, ForceMode.Impulse);
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(throwFrisbeeRight);

            GestureEvent throwFrisbeeLeft = new GestureEvent();
            throwFrisbeeLeft.gestureName = "frisbee throw left";
            throwFrisbeeLeft.onGestureDetected = new UnityEngine.Events.UnityEvent();
            throwFrisbeeLeft.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f && !bowDrawn)
                {
                    GestureFrisbee frisbee = Instantiate(frisbeePrefab);
                    frisbee.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    frisbee.transform.rotation = Camera.main.transform.rotation;

                    Rigidbody rb = frisbee.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * 20f, ForceMode.Impulse);
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(throwFrisbeeLeft);


            /*
            GestureEvent upperCutRight = new GestureEvent();
            upperCutRight.gestureName = "upper cut right hand";
            upperCutRight.onGestureDetected = new UnityEngine.Events.UnityEvent();
            upperCutRight.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f)
                {
                    PulseEffect pulse = Instantiate(pulseEffect);
                    pulse.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    pulse.transform.rotation = Camera.main.transform.rotation;
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(upperCutRight);

            GestureEvent upperCutLeft = new GestureEvent();
            upperCutLeft.gestureName = "upper cut left hand";
            upperCutLeft.onGestureDetected = new UnityEngine.Events.UnityEvent();
            upperCutLeft.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f)
                {
                    PulseEffect pulse = Instantiate(pulseEffect);
                    pulse.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    pulse.transform.rotation = Camera.main.transform.rotation;
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(upperCutLeft);
            */
            GestureEvent throwOverhandRight = new GestureEvent();
            throwOverhandRight.gestureName = "right hand throw overhand";
            throwOverhandRight.onGestureDetected = new UnityEngine.Events.UnityEvent();
            throwOverhandRight.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f && !bowDrawn)
                {
                    GameObject newJavelin = Instantiate(javelinPrefab);
                    newJavelin.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    newJavelin.transform.rotation = Camera.main.transform.rotation;

                    Rigidbody rb = newJavelin.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * 20f, ForceMode.Impulse);
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(throwOverhandRight);

            GestureEvent throwOverhandLeft = new GestureEvent();
            throwOverhandLeft.gestureName = "left hand throw overhand";
            throwOverhandLeft.onGestureDetected = new UnityEngine.Events.UnityEvent();
            throwOverhandLeft.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f && !bowDrawn)
                {
                    GameObject newJavelin = Instantiate(javelinPrefab);
                    newJavelin.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    newJavelin.transform.rotation = Camera.main.transform.rotation;

                    Rigidbody rb = newJavelin.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * 20f, ForceMode.Impulse);
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(throwOverhandLeft);

            GestureEvent crossArmsEvent = new GestureEvent();
            crossArmsEvent.gestureName = "upper cut two hands";
            crossArmsEvent.onGestureDetected = new UnityEngine.Events.UnityEvent();

            crossArmsEvent.onGestureDetected.AddListener(() => ResetScene());
            gestureDetection.AddGestureEvent(crossArmsEvent);

            GestureEvent twoHandThrowOverhand = new GestureEvent();
            twoHandThrowOverhand.gestureName = "two hand throw overhand";
            twoHandThrowOverhand.onGestureDetected = new UnityEngine.Events.UnityEvent();
            twoHandThrowOverhand.onGestureDetected.AddListener(() => 
            {
                if (moveCooldown <= 0f && !bowDrawn)
                {
                    GameObject newJavelin = Instantiate(javelinPrefab);
                    newJavelin.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
                    newJavelin.transform.rotation = Camera.main.transform.rotation;

                    Rigidbody rb = newJavelin.GetComponent<Rigidbody>();
                    rb.AddForce(Camera.main.transform.forward * 20f, ForceMode.Impulse);
                    moveCooldown = moveCooldownTime;
                }
            });
            gestureDetection.AddGestureEvent(twoHandThrowOverhand);
            
        }

        void ResetScene()
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i].transform.position = boxPositions[i];
                boxes[i].transform.rotation = boxRotations[i];
            }

            GameObject[] thrownObjects = GameObject.FindGameObjectsWithTag("ResetObject");
            foreach (GameObject thrownObject in thrownObjects)
            {
                Destroy(thrownObject);
            }

            smashables = FindObjectsOfType<Smashable>();

            foreach (Smashable smashable in smashables)
            {
                if (smashable != null)
                    smashable.ResetObject();
            }
        }

        void DrawBow(string side)
        {
            if (!bowDrawn)
            {
                // if we draw the bow on the right side the bow is held in the left hand and the string/arrow is held in the right hand
                string oppositeHand = side == "right" ? "left" : "right";

                InputDevice stringHand = gestureDetection.GetHandDevice(side);

                // check if grip is held on the string hand
                if (!stringHand.TryGetFeatureValue(CommonUsages.gripButton, out bool gripValue) || !gripValue)
                    return;

                GestureBow bow = Instantiate(bowObjectPrebab);
                bow.SetBowHoldHand(gestureDetection.GetHandTransform(oppositeHand));
                bow.SetStringHoldHand(gestureDetection.GetHandTransform(side));
                bow.SetStringHoldDevice(stringHand);
                bow.SpawnArrow();
                bowDrawn = true;
            }
        }
    }
}