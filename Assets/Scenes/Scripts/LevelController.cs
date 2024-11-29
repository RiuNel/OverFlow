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

    //테스트 변수
    public bool levelChange = false;

    public bool hasUpdated = false;
    public bool hasUpdated2 = false;

    private readonly Vector3 initialTruckPosition = new Vector3(0, 0, 1); // 트럭의 초기 위치


    void Start()
    {
        HideAllLevels();
        ChangeLevel(LevelState.Level1);
    }

    void Update()
    {
        //test 함수
        if (levelChange && currentLevel == LevelState.Level1)
        {
            ChangeLevel(LevelState.Level2);
            Debug.Log("레벨체인지");
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
                if (moveToStep.stop)
                {
                    RunOnce("stop");
                }
                if (moveToStep.finishStop)
                {
                    RunOnce2("finishStop");
                }
                
                if (npcAnimationCoroutine == null) // 이미 실행 중이 아니면 시작
                {
                    npcAnimationCoroutine = StartCoroutine(RepeatNpcChangeAnimation("back"));
                }
                FinishFade(/*초넣기*/);
                break;
            case LevelState.Level2:
                moveToStep.Level2Move(moveToStep.targetPositions[2], moveToStep.targetPositions[3]);
                if (moveToStep.stop)
                {
                    RunOnce("stop");
                }
                if (moveToStep.finishStop)
                {
                    RunOnce2("finishStop");
                }
                if (npcAnimationCoroutine == null) // 이미 실행 중이 아니면 시작 문제가 많음 확실히
                {
                    npcAnimationCoroutine = StartCoroutine(RepeatNpcChangeAnimation("left"));
                }
                FinishFade(/*초넣기*/);
                break;
            case LevelState.Level3:
                moveToStep.Level3Move(moveToStep.targetPositions[4], moveToStep.targetPositions[5]);
                if (moveToStep.stop)
                {
                    RunOnce("stop");
                }
                if (moveToStep.finishStop)
                {
                    RunOnce2("finishStop");
                }
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
                moveToStep.finishStop = false;
                hasUpdated = false;
                hasUpdated2 = false;
                narrationControl.currentNarrationState = NarrationState.Level1Start;
                StartFade();
                break;
            case LevelState.Level2:
                ShowLevel(level2Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                moveToStep.finishStop = false;
                hasUpdated = false;
                hasUpdated2 = false;
                narrationControl.currentNarrationState = NarrationState.Level2Start;
                StartFade();
                break;
            case LevelState.Level3:
                ShowLevel(level3Section);
                InitializeTruckPosition();
                moveToStep.stop = false;
                moveToStep.finishStop = false;
                hasUpdated = false;
                hasUpdated2 = false;
                narrationControl.currentNarrationState = NarrationState.Level3Start;
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
            Debug.Log("코루틴 호출됨");
        }
    }
    IEnumerator ChangeNextLevel()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("왜 실행 됌?");
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
    private void StartFade(float fadeDuration = 2f)
    {
        fadeScreen.GetComponent<FadeScreen>().FadeIn(fadeDuration);
    }
    public void ChangeLevel(LevelState newLevel)
    {
        currentLevel = newLevel;
    }
    

    private IEnumerator RepeatNpcChangeAnimation(string name)
    {
        while (true) // 무한 반복
        {
            // 원하는 애니메이션 변경 (예: back, right, left 중 하나 선택)
            NpcChangeAnimation(name);

            // 2초 대기
            yield return new WaitForSeconds(Timer);
        }
    }

    private void NpcChangeAnimation(string name)
    {
        // 애니메이션 변경
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
    void RunOnce(string stopName)
    {
        if (!hasUpdated)
        {
            switch (currentLevel)
            {
                case LevelState.Level1:
                    if (stopName == "stop")
                    gameObject.GetComponent<NarrationControl>().currentNarrationState = NarrationState.Level1Stop;
                    break;
                case LevelState.Level2:
                    if (stopName == "stop")
                        gameObject.GetComponent<NarrationControl>().currentNarrationState = NarrationState.Level2Stop;
                    break;

                case LevelState.Level3:
                    if (stopName == "stop")
                        gameObject.GetComponent<NarrationControl>().currentNarrationState = NarrationState.Level3Stop;
                    break ;
                default:
                    Debug.LogWarning("알 수 없는 LevelState입니다.");
                    break;
            }
            hasUpdated = true; // 플래그 설정
        }
    }

    void RunOnce2(string stopName)
    {
        if (!hasUpdated2)
        {
            switch (currentLevel)
            {
                case LevelState.Level1:
                    if (stopName == "finishStop")
                    {
                        gameObject.GetComponent<NarrationControl>().currentNarrationState = NarrationState.Level1Finish;
                    }

                    break;

                case LevelState.Level2:
                    
                    if (stopName == "finishStop")
                        gameObject.GetComponent<NarrationControl>().currentNarrationState = NarrationState.Level2Finish;
                    break;

                case LevelState.Level3:
      
                    if (stopName == "finishStop")
                        gameObject.GetComponent<NarrationControl>().currentNarrationState = NarrationState.Level3Finish;
                    break;


                default:
                    Debug.LogWarning("알 수 없는 LevelState입니다.");
                    break;
            }
            hasUpdated2 = true; // 플래그 설정
        }
    }
}
