using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GarbageCtr : MonoBehaviour
{
    public GameObject[] trash;
    //public GameObject trashUI;
    public GameObject first;

    bool isSame;
    public int[] r;
    public Transform[] trashPos;

    public bool isTouch = false;

    /*private float collisionTime; // 충돌이 시작된 시간을 기록
    private float elapsedTime; // 충돌 시간 기록
    private bool isColliding;    // 현재 충돌 중인지 확인
    private float safeTime = 3f;*/


    private void OnEnable()
    {
        isTouch = false;
        r = new int[trashPos.Length];
    }


    void showTrash()
    {
        for (int i = 0; i < trashPos.Length; i++) //쓰레기 3개 추첨
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
        }

        for (int i = 0; i < trashPos.Length; i++)
        {
            Debug.Log(r[i]);
            trash[r[i]].transform.position = trashPos[i].position;
            trash[r[i]].SetActive(true);
        }

        first.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!GameManager.instance.isGrab)
        {
            return;
        }

        if (collision.gameObject.tag == "knife" && !isTouch)
        {
            isTouch = true;
            Invoke("showTrash", 3f);
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "knife")
        {
            isColliding = true;
            elapsedTime = 0f;
            collisionTime = Time.time;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isColliding)
        {
            elapsedTime = Time.time - collisionTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (elapsedTime < safeTime)
        {
            Debug.Log("오염물질 분출");
        }
        else
        {
            Debug.Log("GOOOOOOOOOOOOOOOOOOOOOOOOOD");
        }

        // 충돌이 끝날 때 상태 초기화
        isColliding = false;
        collisionTime = 0f;
        elapsedTime = 0f;
    }*/
}
