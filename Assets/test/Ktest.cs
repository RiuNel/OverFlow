using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ktest : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Ãæµ¹");
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log(contact.point);
        }
    }
}
