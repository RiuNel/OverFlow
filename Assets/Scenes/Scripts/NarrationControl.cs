using System.Collections;
using UnityEngine;

public class NarrationControl : MonoBehaviour
{
    public bool level1_start_narration = false;
    public bool level2_start_narration = false;
    public bool level3_start_narration = false;

    public bool level1_finish_narration = false;
    public bool level2_finish_narration = false;
    public bool level3_finish_narration = false;

    public bool isNarrationPlaying = false; // 내레이션이 진행 중인지 확인

    public GameObject Truck;

    private bool hasTriggeredFinishNarration = false; // 중복 실행 방지

    void Update()
    {
        var moveToStep = Truck.GetComponent<MoveToStep>(); // finishStop 상태 확인

        if (moveToStep == null)
        {
            Debug.LogError("MoveToStep 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // finishStop이 true일 때 종료 내레이션 실행
        if (moveToStep.finishStop && !hasTriggeredFinishNarration)
        {
            TriggerFinishNarration(); // 종료 내레이션 실행
            hasTriggeredFinishNarration = true; // 중복 실행 방지
        }

        CheckStartNarration();
    }

    private void CheckStartNarration()
    {
        if (isNarrationPlaying) return; // 내레이션 중이면 중복 실행 방지

        switch (true)
        {
            case var _ when level1_start_narration:
                StartCoroutine(StartNarration("Level 1 Start Narration", 5f)); // 5초 내레이션
                break;
            case var _ when level2_start_narration:
                StartCoroutine(StartNarration("Level 2 Start Narration", 7f)); // 7초 내레이션
                break;
            case var _ when level3_start_narration:
                StartCoroutine(StartNarration("Level 3 Start Narration", 10f)); // 10초 내레이션
                break;
            default:
                Debug.Log("No start narration active.");
                break;
        }
    }

    private void TriggerFinishNarration()
    {
        if (isNarrationPlaying) return; // 내레이션 중이면 실행 안 함

        switch (true)
        {
            case var _ when level1_finish_narration:
                StartCoroutine(StartNarration("Level 1 Finish Narration", 4f)); // 4초 종료 내레이션
                break;
            case var _ when level2_finish_narration:
                StartCoroutine(StartNarration("Level 2 Finish Narration", 6f)); // 6초 종료 내레이션
                break;
            case var _ when level3_finish_narration:
                StartCoroutine(StartNarration("Level 3 Finish Narration", 8f)); // 8초 종료 내레이션
                break;
            default:
                Debug.Log("No finish narration active.");
                break;
        }
    }

    private IEnumerator StartNarration(string narrationMessage, float duration)
    {
        Debug.Log($"Starting: {narrationMessage}");
        isNarrationPlaying = true;

        // 내레이션 진행 중 작업
        yield return new WaitForSeconds(duration); // 내레이션 진행 시간 기다리기

        Debug.Log($"Ending: {narrationMessage}");
        ResetAllNarration(); // 내레이션 종료 후 상태 초기화
    }

    private void ResetAllNarration()
    {
        level1_start_narration = false;
        level2_start_narration = false;
        level3_start_narration = false;

        level1_finish_narration = false;
        level2_finish_narration = false;
        level3_finish_narration = false;

        isNarrationPlaying = false; // 내레이션 상태 초기화
        hasTriggeredFinishNarration = false; // 종료 내레이션 트리거 초기화
    }
}
