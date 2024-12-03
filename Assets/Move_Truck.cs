using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Transformation;

public class TruckMovement : MonoBehaviour
{
    public Transform[] positions;
    public float speed = 5f;
    public AudioClip movingSound;
    public AudioClip stoppingSound;

    private int currentTargetIndex = 1;
    private bool isStopped = false;
    private float stopTimer = 0f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        PlayMovingSound();
    }

    private void Update()
    {
        if (positions.Length == 0) return;

        if (isStopped)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= 5f)
            {
                isStopped = false;
                PlayMovingSound();
            }
            return;
        }

        Transform target = positions[currentTargetIndex];
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (currentTargetIndex == 1) // Position2에 도착했을 때
            {
                isStopped = true;
                stopTimer = 0f;
                PlayStoppingSound();
            }
            if(currentTargetIndex == 2)
            {
                currentTargetIndex = 0;
                transform.position = positions[0].position;
            }

            currentTargetIndex = (currentTargetIndex + 1) % positions.Length;
        }
    }

    private void PlayMovingSound()
    {
        audioSource.clip = movingSound;
        audioSource.Play();
    }

    private void PlayStoppingSound()
    {
        audioSource.clip = stoppingSound;
        audioSource.Play();
    }
}