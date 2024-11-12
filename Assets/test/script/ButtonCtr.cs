using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtr : MonoBehaviour
{
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        obj.SetActive(!obj.activeSelf);
    }
}
