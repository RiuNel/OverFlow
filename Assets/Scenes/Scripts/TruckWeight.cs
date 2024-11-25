using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckWeight : MonoBehaviour
{
    public int Weight;
    // Start is called before the first frame update
    void Start()
    {
        Weight = Random.Range(4000, 6000);
    }
}
