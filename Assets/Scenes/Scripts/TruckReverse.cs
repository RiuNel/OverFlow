using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Sample;

public class TruckReverse : MonoBehaviour
{
    public bool truckReverseMove = false;
    public float moveSpeed = 2f;   // 이동 속도
    public float rotateSpeed = 10f; // 회전 속도
    public GameObject Truck;

    private GestureCharacterController gestureController;

    private void Start()
    {
        // GestureCharacterController 캐싱
        if (Truck != null)
        {
            gestureController = Truck.GetComponent<GestureCharacterController>();
        }
        else
        {
            Debug.LogError("Truck GameObject가 설정되지 않았습니다.");
        }
    }

    void Update()
    {
        if (gestureController == null) return; // GestureController가 없으면 반환

        //HandleReverseMove(); //그냥 실행 방지
        //HandleRotation(); //그냥 실행 방지 2222
    }

    private void HandleReverseMove()
    {
        if (truckReverseMove)
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);
        }
    }

    public void HandleLeftRotation(float angle)
    {
        float currentY = transform.eulerAngles.y;

        // 각도 보정 (360도 -> -180도 ~ 180도로 변환)
        if (currentY > 180f) currentY -= 360f;

        // 왼쪽 회전 로직
        if (gestureController.truck_move_left)
        {
            if (currentY > -angle) // 왼쪽 회전 허용 각도 확인
            {
                RotateAndMove(-1f); // 왼쪽 방향(-1f)으로 회전 및 이동
            }
        }

        

    }

    public void HandleRightRotation(float angle)
    {
        float currentY = transform.eulerAngles.y;

        // 각도 보정 (360도 -> -180도 ~ 180도로 변환)
        if (currentY > 180f) currentY -= 360f;

        

        // 오른쪽 회전 로직
        if (gestureController.truck_move_right)
        {
            if (currentY < angle) // 오른쪽 회전 허용 각도 확인
            {
                RotateAndMove(1f); // 오른쪽 방향(1f)으로 회전 및 이동
            }
        }

    }


    void RotateAndMove(float direction)
    {
        transform.Rotate(Vector3.up, direction * rotateSpeed * Time.deltaTime, Space.Self); // 회전
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);       // 후진
    }
}
