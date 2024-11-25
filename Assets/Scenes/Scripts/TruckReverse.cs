using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRGestureDetection.Sample;

public class TruckReverse : MonoBehaviour
{
    public bool truckReverseMove = false;
    public float moveSpeed = 2f;   // �̵� �ӵ�
    public float rotateSpeed = 10f; // ȸ�� �ӵ�
    public GameObject Truck;

    private GestureCharacterController gestureController;

    private void Start()
    {
        // GestureCharacterController ĳ��
        if (Truck != null)
        {
            gestureController = Truck.GetComponent<GestureCharacterController>();
        }
        else
        {
            Debug.LogError("Truck GameObject�� �������� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        if (gestureController == null) return; // GestureController�� ������ ��ȯ

        //HandleReverseMove(); //�׳� ���� ����
        //HandleRotation(); //�׳� ���� ���� 2222
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

        // ���� ���� (360�� -> -180�� ~ 180���� ��ȯ)
        if (currentY > 180f) currentY -= 360f;

        // ���� ȸ�� ����
        if (gestureController.truck_move_left)
        {
            if (currentY > -angle) // ���� ȸ�� ��� ���� Ȯ��
            {
                RotateAndMove(-1f); // ���� ����(-1f)���� ȸ�� �� �̵�
            }
        }

        

    }

    public void HandleRightRotation(float angle)
    {
        float currentY = transform.eulerAngles.y;

        // ���� ���� (360�� -> -180�� ~ 180���� ��ȯ)
        if (currentY > 180f) currentY -= 360f;

        

        // ������ ȸ�� ����
        if (gestureController.truck_move_right)
        {
            if (currentY < angle) // ������ ȸ�� ��� ���� Ȯ��
            {
                RotateAndMove(1f); // ������ ����(1f)���� ȸ�� �� �̵�
            }
        }

    }


    void RotateAndMove(float direction)
    {
        transform.Rotate(Vector3.up, direction * rotateSpeed * Time.deltaTime, Space.Self); // ȸ��
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);       // ����
    }
}
