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

    private void Start()
    {
        for (int i = 0; i < GameManager.num; i++)
        {
            result[i] = true;
        }
        finalResult = true;
    }

    public void GoResult()
    {
        for(int i=0; i< GameManager.num; i++)
        {
            if (!GameManager.instance.rTag[i])
            {
                result[i] = false;
                finalResult = false;
            }
        }

        if (finalResult)
        {
            text.text = "GOOD";
        }
        else
        {
            text.text = "BAD";
        }

        resultUI.SetActive(true);

    }

    public void BackResult()
    {
        for (int i = 0; i < GameManager.num; i++)
        {
            if (!GameManager.instance.rTag[i])
            {
                result[i] = false;
                finalResult = false;
            }
        }
        

        if (!finalResult)
        {
            text.text = "GOOD";
        }
        else
        {
            text.text = "BAD";
        }

        resultUI.SetActive(true);
    }

}
