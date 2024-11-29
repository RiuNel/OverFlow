using TMPro;
using UnityEngine;
using VRGestureDetection.Sample;

public class MoveToStep : MonoBehaviour
{
    public GameObject[] targetPositions;
    public GameObject gestureController; //플레이어 제스쳐 인식
    public GameObject Levels;

    private GestureCharacterController gestureControllerCached;
    private Vector3 velocity = Vector3.zero;

    public float truckSpeed = 1f;
    public float positionThreshold = 0.1f; // 목표 지점 도달 거리 임계값

    public bool stop = false;
    public bool finishStop = false;


    void Start()
    {
        // GestureCharacterController 캐싱
        if (gestureController != null)
        {
            gestureControllerCached = gestureController.GetComponent<GestureCharacterController>();
        }
        else
        {
            Debug.LogError("GestureController가 설정되지 않았습니다.");
        }
    }

    void Update()
    {       
        if (gestureControllerCached == null) return; // GestureCharacterController가 없으면 종료
    }

    private void MoveToTarget(Vector3 target)
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, truckSpeed);
    }

    private bool HasReachedTarget(Vector3 currentPosition, Vector3 targetPosition, float threshold)
    {
        return Vector3.Distance(currentPosition, targetPosition) < threshold;
    }


    public void Level1Move(GameObject target1, GameObject target2) 
    {
        if (!stop) 
        {
            if (HasReachedTarget(transform.position, target1.transform.position, 0.1f))
            {
                stop = true;
            }
        }
        
        if (stop && gestureControllerCached.truck_move) // 트럭이 멈춤?
        {
            MoveToTarget(target2.transform.position); // 두 번째 목표 지점으로 이동
        }
        else if (!stop && gestureControllerCached.truck_move)  // 트럭이 안 멈춤? 제스쳐 맞음?
        {
            MoveToTarget(target1.transform.position); // 첫 번째 목표 지점으로 이동
        }

        if (HasReachedTarget(transform.position, target2.transform.position, 0.1f)) //마무리
        {
            Levels.GetComponent<NarrationControl>().level1_finish_narration = true;
            finishStop = true;
            Debug.Log("도착지에 도착");
        }
    }

    public void Level2Move(GameObject target1, GameObject target2)
    {
        if (!stop)
        {
            if (HasReachedTarget(transform.position, target1.transform.position, 0.1f))
            {
                stop = true;
            }
        }
        if (stop && gestureControllerCached.truck_move_left) // 트럭이 멈춤?
        {
            gameObject.GetComponent<TruckReverse>().HandleLeftRotation(90); // 회전한다
        }
        else if (!stop && gestureControllerCached.truck_move)  // 트럭이 안 멈춤? 제스쳐 맞음?
        {
            MoveToTarget(target1.transform.position); // 첫 번째 목표 지점으로 이동
        }
        
        if (HasReachedTarget(transform.position, target2.transform.position, 0.1f)) //마무리
        {
            Levels.GetComponent<NarrationControl>().ResetAllNarration();
            Levels.GetComponent<NarrationControl>().level2_finish_narration = true;
            finishStop = true;
        } 
        
    }

    public void Level3Move(GameObject target1, GameObject target2)
    {
        if (!stop)
        {
            if (HasReachedTarget(transform.position, target1.transform.position, 0.1f))
            {
                stop = true;
            }
        }

        if (stop && gestureControllerCached.truck_move) // 트럭이 멈춤?
        {
            MoveToTarget(target2.transform.position);// 드간다
        }
        if (!stop && gestureControllerCached.truck_move_right)  // 트럭이 안 멈춤? 제스쳐 맞음?
        {
            gameObject.GetComponent<TruckReverse>().HandleRightRotation(45);
             // 첫 번째 목표 지점으로 이동
        }

        if (HasReachedTarget(transform.position, target2.transform.position, 0.1f)) //마무리
        {
            Levels.GetComponent<NarrationControl>().level3_finish_narration = true;
            finishStop = true;
        }

    }
}
