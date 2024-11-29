using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 내레이션 상태를 나타내는 열거형(enum)
public enum NarrationState
{
    None,
    Level1Start,
    Level1Stop,
    Level2Start,
    Level2Stop,
    Level3Start,
    Level3Stop,
    Level1Finish,
    Level2Finish,
    Level3Finish
}

public class NarrationControl : MonoBehaviour
{
    // 내레이션 상태 플래그
    public NarrationState currentNarrationState = NarrationState.None;

    // 내레이션 상태와 관련된 오브젝트들
    public GameObject Truck; // MoveToStep 컴포넌트를 포함하는 오브젝트
    public GameObject Radio; // AudioSource를 포함하는 오브젝트

    private AudioSource audioSource; // 오디오 재생을 담당하는 컴포넌트
    public List<AudioClip> audioClips; // 재생할 오디오 클립 리스트

    private int currentClipIndex = 0; // 현재 재생 중인 클립의 인덱스
    public bool isNarrationPlaying = false; // 내레이션 진행 상태
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


        //TriggerFinishNarration();
        // 시작 내레이션 트리거
        CheckStartNarration();
    }

    // 내레이션 시작을 감지하고 실행
    private void CheckStartNarration()
    {
        // 현재 내레이션이 진행 중이면 실행하지 않음
        if (isNarrationPlaying) return;

        switch (currentNarrationState)
        {
            case NarrationState.Level1Start:
                PlayAudio(0); // audioClips의 0번 인덱스 클립 재생
                break;
            case NarrationState.Level2Start:
                PlayAudio(3);
                break;
            case NarrationState.Level3Start:
                PlayAudio(9, 6, 10);
                break;
            case NarrationState.Level1Stop:
                PlayAudio(1);
                break;
            case NarrationState.Level2Stop:
                PlayAudio(4);
                break;
            case NarrationState.Level3Stop:
                PlayAudio(7);
                break;
            case NarrationState.Level1Finish:
                PlayAudio(2);
                break;
            case NarrationState.Level2Finish:
                PlayAudio(5);
                break;
            case NarrationState.Level3Finish:
                PlayAudio(8);
                break;
            default:
                Debug.Log("내레이션 시작 상태가 설정되지 않았습니다.");
                break;
        }
    }

    // 내레이션 종료를 트리거
    //private void TriggerFinishNarration()
    //{
    //    // 내레이션이 진행 중이면 실행하지 않음
    //    if (isNarrationPlaying) return;

    //    switch (currentNarrationState)
    //    {

    //        default:
    //            Debug.Log("내레이션 종료 상태가 설정되지 않았습니다.");
    //            break;
    //    }

    //    currentNarrationState = NarrationState.None; // 상태 초기화
    //}

    // 모든 내레이션 상태와 플래그를 초기화
    public void ResetAllNarration()
    {
        currentNarrationState = NarrationState.None; // 내레이션 상태 초기화
        isNarrationPlaying = false; // 내레이션 진행 상태 초기화
        hasTriggeredFinishNarration = false; // 종료 내레이션 중복 방지 플래그 초기화
    }

    // 지정된 오디오 클립을 재생
    void PlayAudio(int i)
    {
        isNarrationPlaying = true; //중복 실행 방지 변수
        audioSource.clip = audioClips[i];
        audioSource.Play();
        StartCoroutine(WaitForAudio());
        Debug.Log("PlayAudio 실행");
    }

    void PlayAudio(int i, int j, int k)
    {
        isNarrationPlaying = true; // 중복 실행 방지 변수
        StartCoroutine(PlaySequentialAudio(new int[] { i, j, k }));
    }

    // 순차적으로 오디오를 재생하는 코루틴
    IEnumerator PlaySequentialAudio(int[] clipIndices)
    {
        foreach (int index in clipIndices)
        {
            // 현재 오디오 클립 설정 및 재생
            audioSource.clip = audioClips[index];
            audioSource.Play();

            // 현재 오디오가 끝날 때까지 대기
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        ResetAllNarration(); // 모든 내레이션 플래그 초기화
        isNarrationPlaying = false; // 실행 상태 초기화
        Debug.Log("모든 오디오 재생 완료");
    }

    // 현재 오디오 클립이 끝난 후 내레이션 상태 초기화
    IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        //isNarrationPlaying = false;
        ResetAllNarration(); // 모든 내레이션 플래그 초기화
        Debug.Log("WaitForAudio 실행");
    }
    
}
