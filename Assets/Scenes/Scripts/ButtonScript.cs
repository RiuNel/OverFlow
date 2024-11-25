using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool IsSelectEntered = false;
    public void OnSelectEntered()
    {
        IsSelectEntered = true;
    }

    public void OnSelectExited()
    {
        IsSelectEntered = false;
    }

}
