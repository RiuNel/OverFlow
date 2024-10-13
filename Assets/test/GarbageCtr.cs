using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCtr : MonoBehaviour
{
    public GameObject cutObject;
    public GameObject[] trash;
    public GameObject trashUI;
    public GameObject first;

    bool isSame;
    public int[] r = new int[GameManager.num];

    private void Start()
    {
        for (int i = 0; i < GameManager.num; i++)
        {
            r[i] = 0;
            GameManager.instance.rTag[i] = true;
        }
    }

    void showTrash()
    {
        for (int i = 0; i < GameManager.num; i++)
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

            if (trash[r[i]].tag == "trashBad")
            {
                GameManager.instance.rTag[i] = false;
            }
        }

        for (int i = 0; i < GameManager.num; i++)
        {
            trash[r[i]].SetActive(true);
        }



        trashUI.SetActive(true);

        first.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "knife")
        {
            cutObject.SetActive(true);

            Invoke("showTrash", 3f);
        }

    }
}
