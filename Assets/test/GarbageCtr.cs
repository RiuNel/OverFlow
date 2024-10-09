using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCtr : MonoBehaviour
{
    public GameObject cutObject;
    public GameObject trash;
    public GameObject first;



    void showTrash()
    {
        trash.SetActive(true);
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
