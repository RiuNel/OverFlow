using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GameManager : MonoBehaviour
{
    public SoundManager SoundManager;
    void OnEnable()
    {
        isDone = 0;
    }

    public const int num = 3;
    public bool[] rTag = new bool[num];

    public int hp = 3;
    public GameObject[] hpImage;
    private int iters = 0;
    public GameObject finalresultUI;
    public int isPaint = 0;

    public int isDone = 0;

    public bool isStart = false;
    public bool isFirst = false;
    public bool isGrab = false;
    public bool cutOnce = false;

    public bool pollutionPlay = false;

    public GameObject First;
    public GameObject Second;
    public GameObject hint;

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

    public void FirstOn()
    {
        SoundManager.PlaySFX(SoundManager.ESfx.SFX_trashDisable);
        First.SetActive(false);
        Second.SetActive(true);
    }
    public void HintOpen()
    {
        hint.SetActive(true);
    }

}
