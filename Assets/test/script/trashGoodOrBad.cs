using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class trashGoodOrBad : MonoBehaviour
{
    public GameObject OX;
    private Color color;

    private void Start()
    {
        color = OX.GetComponent<SpriteRenderer>().color;
    }

    public void reMaterial()
    {
        OX.GetComponent<SpriteRenderer>().color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("trash"))
        {
            if (!other.gameObject.GetComponent<isGrabCtr>().isGrab) return;
        }
        else
        //if (other.gameObject.layer != LayerMask.NameToLayer("trash"))
        {
            return;
        }

        if (other.gameObject.tag == this.tag)
        {
            OX.GetComponent<SpriteRenderer>().color = Color.red;
            Debug.Log("-----------------정답");
            Invoke("reMaterial", 1.0f);
            Destroy(other.gameObject);
        }
        else
        {
            OX.GetComponent<SpriteRenderer>().color = Color.black;
            Debug.Log("오답");
            Invoke("reMaterial", 1.0f);
            Destroy(other.gameObject);
        }
    }


}
