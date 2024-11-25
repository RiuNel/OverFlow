using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isGrabCtr : MonoBehaviour
{
    public bool isGrab = false;
    public void isGrabOn()
    {
        isGrab = true;
    }
    public void isGrabOff()
    {
        isGrab = false;
    }

    private void OnEnable()
    {
        isGrab = false;
    }
}
