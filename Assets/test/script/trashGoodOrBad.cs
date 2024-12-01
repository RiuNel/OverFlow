using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class trashGoodOrBad : MonoBehaviour
{
    public GameManager GameManager;
    public SoundManager SoundManager;

    public GameObject OX;
    private Color color;

    private void OnEnable()
    {
        color = OX.GetComponent<SpriteRenderer>().color;
    }


    public void reMaterial()
    {
        OX.GetComponent<SpriteRenderer>().color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrabCtr gc = other.gameObject.GetComponent<isGrabCtr>();
        if (gc != null && !gc.isGrab)
        {
            return;
        }
        if (other.gameObject.layer != LayerMask.NameToLayer("trash"))
        {
            return;
        }

        if (other.gameObject.tag == this.tag)
        {
            GameManager.isDone++;
            SoundManager.PlaySFX(SoundManager.ESfx.SFX_trashDisable);
            OX.GetComponent<SpriteRenderer>().color = Color.red;
            SoundManager.PlayRandomNarration(SoundManager.goodTrash);
            Debug.Log("-----------------정답");
            Invoke("reMaterial", 1.0f);
            Destroy(other.gameObject);
        }
        else
        {
            GameManager.isDone++;
            SoundManager.PlaySFX(SoundManager.ESfx.SFX_trashDisable);
            //OX.GetComponent<SpriteRenderer>().color = Color.black;
            SoundManager.PlayRandomNarration(SoundManager.badTrash);
            Debug.Log("오답");
            //Invoke("reMaterial", 1.0f);
            Destroy(other.gameObject);
        }
    }


}
