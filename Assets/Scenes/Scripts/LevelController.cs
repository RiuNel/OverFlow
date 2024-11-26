using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Coroutine npcAnimationCoroutine;
    public enum LevelState { None, Level1, Level2, Level3 }
    public LevelState currentLevel = LevelState.None;
    private LevelState previousLevel = LevelState.None;

    public GameObject fadeScreen;
    public GameObject npc;
    public float Timer = 2.0f;
    public bool isTouchingTruck = false;
    private bool hasFadedOut = false;
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
                if (npcAnimationCoroutine == null) // �̹� ���� ���� �ƴϸ� ����
                {
                    npcAnimationCoroutine = StartCoroutine(RepeatNpcChangeAnimation("back"));
                }
                FinishFade(/*�ʳֱ�*/);
                break;
            case LevelState.Level2:
                moveToStep.Level2Move(moveToStep.targetPositions[2], moveToStep.targetPositions[3]);
                if (npcAnimationCoroutine == null) // �̹� ���� ���� �ƴϸ� ���� ������ ���� Ȯ����
                {
                    npcAnimationCoroutine = StartCoroutine(RepeatNpcChangeAnimation("left"));
                }
                FinishFade(/*�ʳֱ�*/);
                break;
            case LevelState.Level3:
                moveToStep.Level3Move(moveToStep.targetPositions[4], moveToStep.targetPositions[5]);
                FinishFade(/*�ʳֱ�*/);
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
                StartFade();
                break;
            case LevelState.Level2:
                ShowLevel(level2Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                narrationControl.level2_start_narration = true;
                StartFade();
                break;
            case LevelState.Level3:
                ShowLevel(level3Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                narrationControl.level3_start_narration = true;
                StartFade();
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

    private void FinishFade(float fadeDuration = 2f)
    {
        if (isTouchingTruck && !hasFadedOut)
        {
            fadeScreen.GetComponent<FadeScreen>().FadeOut(fadeDuration);
            hasFadedOut = true; // �� �� ���������� ���
            StartCoroutine(ChangeNextLevel());
        }
    }

    private void StartFade(float fadeDuration = 2f)
    {

        fadeScreen.GetComponent<FadeScreen>().FadeIn(fadeDuration);

    }
    public void ChangeLevel(LevelState newLevel)
    {
        currentLevel = newLevel;
    }
    IEnumerator ChangeNextLevel()
    {
        yield return new WaitForSeconds(2f);
        hasFadedOut = false;
        switch (currentLevel)
        {
            case LevelState.None:
                currentLevel = LevelState.Level1;
                break;
            case LevelState.Level1:
                currentLevel = LevelState.Level2;
                break;
            case LevelState.Level2:
                currentLevel = LevelState.Level3;
                break;
            case LevelState.Level3:
                break;
        }
    }

    private IEnumerator RepeatNpcChangeAnimation(string name)
    {
        while (true) // ���� �ݺ�
        {
            // ���ϴ� �ִϸ��̼� ���� (��: back, right, left �� �ϳ� ����)
            NpcChangeAnimation(name);

            // 2�� ���
            yield return new WaitForSeconds(Timer);
        }
    }

    private void NpcChangeAnimation(string name)
    {
        // �ִϸ��̼� ����
        switch (name)
        {
            case "back":
                npc.GetComponent<AniTestScript>().BackAnimation();
                break;
            case "right":
                npc.GetComponent<AniTestScript>().RightAnimation();
                break;
            case "left":
                npc.GetComponent<AniTestScript>().LeftAnimation();
                break;
        }
    }
}
