using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class trashGoodOrBad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("trash")) { return; }

        if (other.gameObject.tag == this.tag)
        {
            Debug.Log("-----------------정답");
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("오답");
            Destroy(other.gameObject);
        }
    }
}
