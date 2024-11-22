using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class trashCtr : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool[] result = new bool[GameManager.num];
    public bool finalResult;
    public GameObject resultUI;

    public GameObject[] trash;
    public GameObject trashUI;
    public GameObject first;

    private void OnEnable()
    {
        for (int i = 0; i < GameManager.num; i++)
        {
            result[i] = true;
        }
        finalResult = true;
    }

    private void Update()
    {
        if (GameManager.instance.hp <= 0)
        {
            GameManager.instance.hpEnd();
            Invoke("ReGame", 3f);
        }
    }

    public void GoResult() //Go를 눌렀을 때
    {
        for(int i=0; i< GameManager.num; i++)
        {
            if (!GameManager.instance.rTag[i]) //Bed가 존재하면 
            {
                result[i] = false;
                finalResult = false;
            }
        }

        if (finalResult) //Bed가 없음
        {
            text.text = "GOOD";
        }
        else //Bed가 있음
        {
            text.text = "BAD";
            GameManager.instance.hpDown();
            //Invoke("ReGame", 3f);
        }

        resultUI.SetActive(true);

    }

    public void BackResult() //Back을 눌렀을 때
    {
        for (int i = 0; i < GameManager.num; i++)
        {
            if (!GameManager.instance.rTag[i]) //Bed가 존재하면 
            {
                result[i] = false;
                finalResult = false;
            }
        }
        

        if (!finalResult) //Bed가 있음
        {
            text.text = "GOOD";
        }
        else //Bed가 없음
        {
            text.text = "BAD";
            GameManager.instance.hpDown();
            //Invoke("ReGame", 3f);
        }

        resultUI.SetActive(true);
    }

    public void ReGame()
    {
        for(int i = 0; i < trash.Length; i++)
        {
            trash[i].SetActive(false);
        }
        trashUI.SetActive(false);
        resultUI.SetActive(false);
        first.SetActive(true);
    }

}
