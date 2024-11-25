using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public const int num = 3;
    public bool[] rTag = new bool[num];

    public int hp = 3;
    public GameObject[] hpImage;
    private int iters = 0;
    public GameObject finalresultUI;
    public int isPaint = 0;

    public bool isGrab = false;
    public void isGrabOn()
    {
        isGrab = true;
    }
    public void isGrabOff()
    {
        isGrab = false;
    }

    public void hpDown()
    {
        hp--;
        hpImage[iters].SetActive(false);
        iters++;
    }
    public void hpEnd()
    {
        finalresultUI.SetActive(true);
        iters = 0;
        for(int i = 0; i < num; i++)
        {
            hpImage[i].SetActive(true);
        }
    }
}
