using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GarbageCtr : MonoBehaviour
{
    public GameObject cutObject;
    public GameObject[] trash;
    public GameObject trashUI;
    public GameObject first;

    bool isSame;
    public int[] r = new int[GameManager.num];
    public Transform[] trashPos;

    public bool isTouch = false;

   private void OnEnable()
    {
        cutObject.SetActive(false);
        for (int i = 0; i < GameManager.num; i++)
        {
            r[i] = 0;
            GameManager.instance.rTag[i] = true;
        }

        GameManager.instance.isPaint = 0;
        isTouch = false;
   }


    void showTrash()
    {
        for (int i = 0; i < GameManager.num; i++) //쓰레기 3개 추첨
        {
            do
            {
                isSame = false;
                r[i] = Random.Range(0, trash.Length);

                for (int j = 0; j < i; j++)
                {
                    if (r[i] == r[j])
                    {
                        isSame = true;
                        break;
                    }
                }
            } while (isSame);

            if (trash[r[i]].tag == "trashBad") //Bed -> false
            {
                GameManager.instance.rTag[i] = false;
            }
        }

        for (int i = 0; i < GameManager.num; i++)
        {
            trash[r[i]].transform.position = trashPos[i].position;
            trash[r[i]].SetActive(true);
        }



        trashUI.SetActive(true);

        first.SetActive(false);
    }

    /*private void paintOK()
    {
        // 2초 이상
        if(GameManager.instance.isPaint == 1 && !isTouch)
        {
            isTouch = true;
            Debug.Log("성공");
            Invoke("showTrash", 3f);
        }
        // 2초 이하
        else if (GameManager.instance.isPaint == 2 && !isTouch)
        {
            isTouch = true;
            GameManager.instance.hpDown();
            Debug.Log("오염물질 분출");
            Invoke("showTrash", 3f);
        }
    }*/


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "knife" && !isTouch)
        {
            isTouch = true;
            Invoke("showTrash", 5f);
        }
    }

    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "knife" && !isTouch)
        {
            isTouch = true;
            cutObject.SetActive(true);
            Invoke("showTrash", 3f);
        }

    }*/
}
