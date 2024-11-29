using TMPro;
using UnityEngine;
using VRGestureDetection.Sample;

public class MoveToStep : MonoBehaviour
{
    public GameObject[] targetPositions;
    public GameObject gestureController; //�÷��̾� ������ �ν�
    public GameObject Levels;

    private GestureCharacterController gestureControllerCached;
    private Vector3 velocity = Vector3.zero;

    public float truckSpeed = 1f;
    public float positionThreshold = 0.1f; // ��ǥ ���� ���� �Ÿ� �Ӱ谪

    public bool stop = false;
    public bool finishStop = false;


    void Start()
    {
        // GestureCharacterController ĳ��
        if (gestureController != null)
        {
            gestureControllerCached = gestureController.GetComponent<GestureCharacterController>();
        }
        else
        {
            Debug.LogError("GestureController�� �������� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {       
        if (gestureControllerCached == null) return; // GestureCharacterController�� ������ ����
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
        
        if (stop && gestureControllerCached.truck_move) // Ʈ���� ����?
        {
            MoveToTarget(target2.transform.position); // �� ��° ��ǥ �������� �̵�
        }
        else if (!stop && gestureControllerCached.truck_move)  // Ʈ���� �� ����? ������ ����?
        {
            MoveToTarget(target1.transform.position); // ù ��° ��ǥ �������� �̵�
        }

        if (HasReachedTarget(transform.position, target2.transform.position, 0.1f)) //������
        {
            Levels.GetComponent<NarrationControl>().level1_finish_narration = true;
            finishStop = true;
            Debug.Log("�������� ����");
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
        if (stop && gestureControllerCached.truck_move_left) // Ʈ���� ����?
        {
            gameObject.GetComponent<TruckReverse>().HandleLeftRotation(90); // ȸ���Ѵ�
        }
        else if (!stop && gestureControllerCached.truck_move)  // Ʈ���� �� ����? ������ ����?
        {
            MoveToTarget(target1.transform.position); // ù ��° ��ǥ �������� �̵�
        }
        
        if (HasReachedTarget(transform.position, target2.transform.position, 0.1f)) //������
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

        if (stop && gestureControllerCached.truck_move) // Ʈ���� ����?
        {
            MoveToTarget(target2.transform.position);// �尣��
        }
        if (!stop && gestureControllerCached.truck_move_right)  // Ʈ���� �� ����? ������ ����?
        {
            gameObject.GetComponent<TruckReverse>().HandleRightRotation(45);
             // ù ��° ��ǥ �������� �̵�
        }

        if (HasReachedTarget(transform.position, target2.transform.position, 0.1f)) //������
        {
            Levels.GetComponent<NarrationControl>().level3_finish_narration = true;
            finishStop = true;
        }

    }
}
