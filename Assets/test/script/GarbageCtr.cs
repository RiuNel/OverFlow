using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GarbageCtr : MonoBehaviour
{
    public GameManager GameManager;
    public SoundManager SoundManager;

    public GameObject[] trash;
    //public GameObject trashUI;
    public GameObject first;

    bool isSame;
    public int[] r;
    public Transform[] trashPos;

    public bool isTouch = false;


    private void OnEnable()
    {
        isTouch = false;
        r = new int[trashPos.Length];
    }


    void showTrash()
    {
        if (!GameManager.isFirst)
        {
            GameManager.isFirst = true;
            GameManager.pollutionPlay = true;
            return;
        }

        if (!GameManager.cutOnce && GameManager.isFirst) { GameManager.cutOnce = true; }

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

        SoundManager.PlaySFX(SoundManager.ESfx.SFX_trashDisable);
        first.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!GameManager.isGrab)
        {
            return;
        }

        if (collision.gameObject.tag == "knife" && !isTouch)
        {
            isTouch = true;
            Invoke("showTrash", 3f);
        }
    }
}
