using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public enum LevelState { None, Level1, Level2, Level3 }
    public LevelState currentLevel = LevelState.None;
    private LevelState previousLevel = LevelState.None;

    public GameObject[] level1Section;
    public GameObject[] level2Section;
    public GameObject[] level3Section;
    public GameObject Truck;

    //�׽�Ʈ ����
    public bool levelChange = false;    

    private readonly Vector3 initialTruckPosition = new Vector3(0, 0, 1); // Ʈ���� �ʱ� ��ġ
    

    void Start()
    {
        HideAllLevels();
        //gameObject.GetComponent<NarrationControl>().start_narration = true;
        ChangeLevel(LevelState.Level1);
    }

    void Update()
    {
        //test �Լ�
        if (levelChange && currentLevel == LevelState.Level1)
        {
            ChangeLevel(LevelState.Level2);
            levelChange = false;
        }
        if (levelChange && currentLevel == LevelState.Level2)
        {
            ChangeLevel(LevelState.Level3);
            levelChange = false;
        }


        var moveToStep = Truck.GetComponent<MoveToStep>(); //Ʈ�� �ȿ� �ִ� ��ũ��Ʈ ���Ŀ���
        var narrationControl = gameObject.GetComponent<NarrationControl>(); //�����̼� ��Ʈ�� ��ũ��Ʈ ��������

        if (moveToStep == null)
        {
            Debug.LogError("MoveToStep ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        if (currentLevel != previousLevel)
        {
            HandleLevelChange(moveToStep, narrationControl); //������ �ٲ� �� ���� ������ ����
            previousLevel = currentLevel;  //Update ���ϱ� ���� ������ ����
        }

        switch (currentLevel)
        {
            case LevelState.Level1:
                moveToStep.Level1Move(moveToStep.targetPositions[0], moveToStep.targetPositions[1]);
                break;
            case LevelState.Level2:
                moveToStep.Level2Move(moveToStep.targetPositions[2], moveToStep.targetPositions[3]);
                break;
            case LevelState.Level3:
                moveToStep.Level3Move(moveToStep.targetPositions[4], moveToStep.targetPositions[5]);
                break;
        }
    }

    private void HandleLevelChange(MoveToStep moveToStep, NarrationControl narrationControl)
    {
        switch (currentLevel)
        {
            case LevelState.Level1:
                ShowLevel(level1Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                narrationControl.level1_start_narration = true;
                
                break;
            case LevelState.Level2:
                ShowLevel(level2Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                narrationControl.level2_start_narration = true;

                break;
            case LevelState.Level3:
                ShowLevel(level3Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                narrationControl.level3_start_narration = true;

                break;
            default:
                HideAllLevels();
                InitializeTruckPosition();
                moveToStep.stop = false;
                break;
        }
    }

    private void InitializeTruckPosition()
    {
        Truck.transform.position = initialTruckPosition; // �ʱ� ��ġ�� �̵�
        Truck.transform.rotation = Quaternion.identity; // �⺻ ȸ����
    }

    private void HideAllLevels()
    {
        HideObjects(level1Section);
        HideObjects(level2Section);
        HideObjects(level3Section);
    }

    private void ShowLevel(GameObject[] levelSections)
    {
        HideAllLevels(); // �ٸ� ���� �����
        ShowObjects(levelSections);
    }

    private void HideObjects(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    private void ShowObjects(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj != null) obj.SetActive(true);
        }
    }

    public void ChangeLevel(LevelState newLevel)
    {
        currentLevel = newLevel;
    }
}