using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Sample;


public class SectionChangeColor : MonoBehaviour
{
    public Transform Truck;
    public float tolerance = 0.5f;
    private Renderer capsuleColor;
    public GameObject gestureController;
    private bool onceDistance = false;

    void Start()
    {
        capsuleColor = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        float distance = Vector3.Distance(Truck.position, gameObject.transform.position);



        if (distance <= tolerance && !onceDistance)
        {

            capsuleColor.material.color = Color.white; // 트럭이 가까우면 흰색
            Debug.Log("트럭이 가까워졌습니다! 흰색으로 변경");
            onceDistance = true;
            gestureController.GetComponent<GestureCharacterController>().TruckIdleReset();
        }



    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tolerance); // 허용 범위 시각화
    }
}
