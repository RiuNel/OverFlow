using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using VRGestureDetection.Sample;
using static UnityEngine.GraphicsBuffer;

public class WheelRotation : MonoBehaviour
{
    public GameObject[] wheelObjects; // ������ GameObject �迭�� �ޱ�
    public GameObject[] frontWheelObjects;
    public GameObject Truck;
    private Transform[] wheelTransforms;
    private Transform[] frontWheelTransforms;

    public bool Rotate;
    public bool TestRotate;


    //private bool isMoving;

    public float rotationSpeed = 200; // ���� ȸ�� �ӵ�
    public float frontrotationSpeed = 1f;

    private void Start()
    {
        wheelTransforms = new Transform[wheelObjects.Length];
        for (int i = 0; i < wheelObjects.Length; i++)
        {
            wheelTransforms[i] = wheelObjects[i].transform;
        }

        // �չ��� Transform �迭�� ��ȯ
        frontWheelTransforms = new Transform[frontWheelObjects.Length];
        for (int i = 0; i < frontWheelObjects.Length; i++)
        {
            frontWheelTransforms[i] = frontWheelObjects[i].transform;
        }
    }

    private void Update()
    {
        if (Truck.GetComponent<MoveToStep>().stop == false)
        {
            Wheelrotation();
        }

        frontWheelrotation();
    }



    void Wheelrotation()
    {
        float speed = rotationSpeed * Time.deltaTime;

        //��� ������ �յ� ȸ�� ����
        foreach (Transform wheel in wheelTransforms)
        {
            float currentX = wheel.eulerAngles.x;

            float targetX = currentX + speed;
            wheel.rotation = Quaternion.Euler(wheel.eulerAngles.x, targetX, wheel.eulerAngles.x);
            //wheel.Rotate(Vector3.right, speed, Space.Self);
        }
    }

    void frontWheelrotation()
    {
        //�� ���� ���ư��� �ӵ�
        float frontspeed = frontrotationSpeed * Time.deltaTime;
        frontrotationSpeed = TestRotate ? 40 : -40;

        if (Rotate)
        {
            foreach (Transform frontWheel in frontWheelTransforms)
            {
                float currentY = frontWheel.eulerAngles.y;

                float targetY = currentY + frontspeed;
                // ��ǥ ���� ���� (330���� 30�� ���̷� ����)
                if (targetY > 330) targetY -= 330; // 360�� �ʰ� �� 0���� �ǵ���
                if (targetY < 0) targetY += 330;  // 0�� �̸� �� 360���� �ǵ���

                // 330~360�� �Ǵ� 0~30�� ���̷� ����
                if (targetY > 30 && targetY < 330)
                {
                    targetY = (targetY >= 180) ? 330 : 30; // �߰��� �������� ����� ������ ����
                }


                // Quaternion�� ����� Y�� ȸ�� ����
                if (TestRotate)
                {
                    frontWheel.rotation = Quaternion.Euler(frontWheel.eulerAngles.x, targetY, frontWheel.eulerAngles.z);
                }
                else 
                {
                    frontWheel.rotation = Quaternion.Euler(frontWheel.eulerAngles.x, targetY, frontWheel.eulerAngles.z);
                }




                /*
                frontWheel.Rotate(Vector3.up, frontspeed, Space.World);

                // �չ����� ���� Y�� ȸ�� ���� ���
                float yRotation = frontWheel.eulerAngles.y;
                Debug.Log(yRotation);
                //330�� 30�� �־���Ѵ�.

                */
            }
        }
    }
}
