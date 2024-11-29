using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationControl : MonoBehaviour
{
    // 내레이션 시작 상태 플래그
    public bool level1_start_narration = false;
    public bool level1_stop_narration = false;
    public bool level2_start_narration = false;
    public bool level2_stop_narration = false;
    public bool level3_start_narration = false; 
    public bool level3_stop_narration = false;

    // 내레이션 종료 상태 플래그
    public bool level1_finish_narration = false;
    public bool level2_finish_narration = false;
    public bool level3_finish_narration = false;

    // 내레이션이 현재 진행 중인지 여부
    public bool isNarrationPlaying = false;

    // 내레이션 상태와 관련된 오브젝트들
    public GameObject Truck; // MoveToStep 컴포넌트를 포함하는 오브젝트
    public GameObject Radio; // AudioSource를 포함하는 오브젝트

    private AudioSource audioSource; // 오디오 재생을 담당하는 컴포넌트
    public List<AudioClip> audioClips; // 재생할 오디오 클립 리스트

    private int currentClipIndex = 0; // 현재 재생 중인 클립의 인덱스
    private bool hasTriggeredFinishNarration = false; // 종료 내레이션 중복 실행 방지

    private void Start()
    {
        // Radio가 설정되었는지 확인
        if (Radio == null)
        {
            Debug.LogError("Radio가 할당되지 않았습니다. Unity 인스펙터에서 설정하세요.");
            return;
        }

        // Radio에서 AudioSource 컴포넌트를 가져옴
        audioSource = Radio.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Radio 객체에 AudioSource 컴포넌트가 없습니다. AudioSource를 추가하세요.");
        }

        // Truck 오브젝트가 설정되었는지 확인
        if (Truck == null)
        {
            Debug.LogError("Truck 객체가 할당되지 않았습니다. Unity 인스펙터에서 설정하세요.");
        }
    }

    void Update()
    {
        // Truck에서 MoveToStep 컴포넌트 가져오기
        var moveToStep = Truck?.GetComponent<MoveToStep>();
        if (moveToStep == null)
        {
            Debug.LogError("Truck에 MoveToStep 컴포넌트가 없습니다. 추가하거나 오브젝트를 확인하세요.");
            return;
        }

        // 종료 내레이션 트리거: finishStop 상태가 true일 때 실행
        if (moveToStep.finishStop && !hasTriggeredFinishNarration)
        {
            TriggerFinishNarration(); // 종료 내레이션 실행
            hasTriggeredFinishNarration = true; // 중복 실행 방지
        }

        // 시작 내레이션 트리거
        CheckStartNarration();
    }

    // 내레이션 시작을 감지하고 실행
    private void CheckStartNarration()
    {
        // 현재 내레이션이 진행 중이면 실행하지 않음
        if (isNarrationPlaying) return;

        // 내레이션 시작 플래그에 따라 적절한 내레이션 실행
        if (level1_start_narration)
        {
            PlayAudio(0); // audioClips의 0번 인덱스 클립 재생
        }
        if (level2_start_narration)
        {
            PlayAudio(3);
        }
        else if (level3_start_narration)
        {
            PlayAudio(6);
        }
        else if (level1_stop_narration)
        {
            PlayAudio(1);
        }
        else if (level2_stop_narration)
        {
            PlayAudio(4);
        }
        else if (level3_stop_narration)
        {
            PlayAudio(1);
        }
        else
        {
            Debug.Log("내레이션 시작 플래그가 설정되지 않았습니다.");
        }
        


        
    }

    // 내레이션 종료를 트리거
    private void TriggerFinishNarration()
    {
        // 내레이션이 진행 중이면 실행하지 않음
        if (isNarrationPlaying) return;

        if (level1_finish_narration)
        {
            PlayAudio(2);
        }
        else if (level2_finish_narration)
        {
            PlayAudio(5);
            PlayAudio(6);
        }
        else if (level3_finish_narration)
        {
            PlayAudio(8);
        }
        else
        {
            Debug.Log("내레이션 종료 플래그가 설정되지 않았습니다.");
        }
    }

    // 모든 내레이션 상태와 플래그를 초기화
    public void ResetAllNarration()
    {
        // 시작 플래그 초기화
        level1_start_narration = false;
        level1_stop_narration = false;
        level2_start_narration = false;
        level3_start_narration = false;

        // 종료 플래그 초기화
        level1_finish_narration = false;
        level2_finish_narration = false;
        level3_finish_narration = false;

        isNarrationPlaying = false; // 내레이션 상태 초기화
        hasTriggeredFinishNarration = false; // 종료 내레이션 중복 방지 플래그 초기화
    }

    // 지정된 범위의 오디오 클립을 재생
    void PlayAudio(int min, int max)
    {
        isNarrationPlaying = true;
        // audioClips 및 범위 유효성 검사
        if (audioClips == null || audioClips.Count == 0 || min < 0 || max >= audioClips.Count)
        {
            Debug.LogWarning("오디오 클립 리스트가 비어 있거나 인덱스 범위가 잘못되었습니다.");
            return;
        }

        // 현재 클립 인덱스 설정
        currentClipIndex = min;
        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();

        // 현재 클립이 끝나면 다음 클립 재생
        StartCoroutine(WaitForAudioToEnd(min, max));
    }

    void PlayAudio(int i)
    {
        isNarrationPlaying = true;
        audioSource.clip = audioClips[i];
        audioSource.Play();
        StartCoroutine(WaitForAudio());
    }

    // 현재 오디오 클립이 끝난 후 다음 클립으로 넘어감
    IEnumerator WaitForAudioToEnd(int min, int max)
    {
        // 현재 클립의 길이만큼 대기
        yield return new WaitForSeconds(audioSource.clip.length);

        // 다음 클립으로 이동
        currentClipIndex++;
        
        if (currentClipIndex <= max)
        {
            PlayAudio(currentClipIndex, max); // 재귀 호출로 다음 클립 재생
        }
        else
        {
            Debug.Log("끝 나레이션");
            isNarrationPlaying = false;
            ResetAllNarration(); // 모든 내레이션 플래그 초기화
        }
    }
    IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        isNarrationPlaying = false;
        ResetAllNarration(); // 모든 내레이션 플래그 초기화
    }
}
