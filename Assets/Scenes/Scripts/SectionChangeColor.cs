using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Sample;


public class SectionChangeColor : MonoBehaviour
{
    public Transform Truck;
    public float tolerance = 0.5f;
    public GameObject Levels;

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
            capsuleColor.material.color = Color.white; // Ʈ���� ������ ���
            Debug.Log("Ʈ���� ����������ϴ�! ������� ����");
            onceDistance = true;
            gestureController.GetComponent<GestureCharacterController>().TruckIdleReset();
            if (gameObject.name == "Section2")
            {
                Levels.GetComponent<LevelController>().isTouchingTruck = true;
                StartCoroutine(ResetTouchState());
            } 
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tolerance); // ��� ���� �ð�ȭ
    }

    IEnumerator ResetTouchState()
    {
        yield return new WaitForSeconds(2f); // 2�� ���
        Levels.GetComponent<LevelController>().isTouchingTruck = false;
    }
}
