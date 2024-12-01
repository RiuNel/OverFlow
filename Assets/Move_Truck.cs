using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move_Truck : MonoBehaviour
{
    public Transform First_Point;
    public Transform Second_Point;
    public Transform Last_Point;
    public int Point = 1;
    public AudioSource _audio;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Second_Point = gameObject.transform;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    IEnumerator SetPoint(float time)
    {
        yield return new WaitForSeconds(time);
        Point++;
        
    }

    IEnumerator Move()
    {
        if (Point == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, Second_Point.position, 0.001f * Time.deltaTime);
            if (transform.position == Second_Point.position)
            {
                _audio.clip = clips[0];
                _audio.Play();
                StartCoroutine(SetPoint(5.0f));
            }
        }
        else if (Point == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, Last_Point.position, 0.001f * Time.deltaTime);
            _audio.clip = clips[1];
            _audio.Play();
            StartCoroutine(SetPoint(0.1f));
        }
        else if (Point == 2)
        {
            transform.position = First_Point.position;
            Point = 0;
        }
        StartCoroutine(Move());

        yield return null;
    }
}
