using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using VRGestureDetection.Sample;
using static UnityEngine.GraphicsBuffer;

public class WheelRotation : MonoBehaviour
{
    public GameObject[] wheelObjects; // 바퀴를 GameObject 배열로 받기
    public GameObject[] frontWheelObjects;
    public GameObject Truck;
    private Transform[] wheelTransforms;
    private Transform[] frontWheelTransforms;

    public bool Rotate;
    public bool TestRotate;


    //private bool isMoving;

    public float rotationSpeed = 200; // 바퀴 회전 속도
    public float frontrotationSpeed = 1f;

    private void Start()
    {
        wheelTransforms = new Transform[wheelObjects.Length];
        for (int i = 0; i < wheelObjects.Length; i++)
        {
            wheelTransforms[i] = wheelObjects[i].transform;
        }

        // 앞바퀴 Transform 배열로 변환
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

        //모든 바퀴의 앞뒤 회전 적용
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
        //앞 바퀴 돌아가는 속도
        float frontspeed = frontrotationSpeed * Time.deltaTime;
        frontrotationSpeed = TestRotate ? 40 : -40;

        if (Rotate)
        {
            foreach (Transform frontWheel in frontWheelTransforms)
            {
                float currentY = frontWheel.eulerAngles.y;

                float targetY = currentY + frontspeed;
                // 목표 각도 설정 (330도와 30도 사이로 제한)
                if (targetY > 330) targetY -= 330; // 360도 초과 시 0도로 되돌림
                if (targetY < 0) targetY += 330;  // 0도 미만 시 360도로 되돌림

                // 330~360도 또는 0~30도 사이로 제한
                if (targetY > 30 && targetY < 330)
                {
                    targetY = (targetY >= 180) ? 330 : 30; // 중간값 기준으로 가까운 쪽으로 조정
                }


                // Quaternion을 사용해 Y축 회전 적용
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

                // 앞바퀴의 현재 Y축 회전 각도 출력
                float yRotation = frontWheel.eulerAngles.y;
                Debug.Log(yRotation);
                //330과 30을 넣어야한다.

                */
            }
        }
    }
}
