using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public enum LevelState { None, Level1, Level2, Level3 }
    public LevelState currentLevel = LevelState.None;
    private LevelState previousLevel = LevelState.None;

    public GameObject fadeScreen;
    public bool isTouchingTruck = false;
    private bool hasFadedOut = false;
    public GameObject[] level1Section;
    public GameObject[] level2Section;
    public GameObject[] level3Section;
    public GameObject Truck;

    //테스트 변수
    public bool levelChange = false;

    private readonly Vector3 initialTruckPosition = new Vector3(0, 0, 1); // 트럭의 초기 위치


    void Start()
    {
        HideAllLevels();
        //gameObject.GetComponent<NarrationControl>().start_narration = true;
        ChangeLevel(LevelState.Level1);
    }

    void Update()
    {
        //test 함수
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


        var moveToStep = Truck.GetComponent<MoveToStep>(); //트럭 안에 있는 스크립트 훔쳐오기
        var narrationControl = gameObject.GetComponent<NarrationControl>(); //나레이션 컨트롤 스크립트 가져오기

        if (moveToStep == null)
        {
            Debug.LogError("MoveToStep 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        if (currentLevel != previousLevel)
        {
            HandleLevelChange(moveToStep, narrationControl); //레벨이 바뀔 때 레벨 디자인 수정
            previousLevel = currentLevel;  //Update 피하기 위한 수정용 변수
        }

        switch (currentLevel)
        {
            case LevelState.Level1:
                moveToStep.Level1Move(moveToStep.targetPositions[0], moveToStep.targetPositions[1]);
                FinishFade(/*초넣기*/);
                break;
            case LevelState.Level2:
                moveToStep.Level2Move(moveToStep.targetPositions[2], moveToStep.targetPositions[3]);
                FinishFade(/*초넣기*/);
                break;
            case LevelState.Level3:
                moveToStep.Level3Move(moveToStep.targetPositions[4], moveToStep.targetPositions[5]);
                FinishFade(/*초넣기*/);
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
        Truck.transform.position = initialTruckPosition; // 초기 위치로 이동
        Truck.transform.rotation = Quaternion.identity; // 기본 회전값
    }

    private void HideAllLevels()
    {
        HideObjects(level1Section);
        HideObjects(level2Section);
        HideObjects(level3Section);
    }

    private void ShowLevel(GameObject[] levelSections)
    {
        HideAllLevels(); // 다른 섹션 숨기기
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
            hasFadedOut = true; // 한 번 실행했음을 기록
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
}
