using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickMenu : MonoBehaviour
{
    public GameObject menu;
    public void Back()
    {
        menu.SetActive(false);
    }
}
