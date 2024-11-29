using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;
using static Unity.Barracuda.TextureAsTensorData;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //BGM 종류들
    public enum ENarration
    {
        audio_24,
        audio_25,
        audio_26,
        audio_27,
        audio_28,
        audio_29,
        audio_30,
        audio_31,
        audio_32,
        audio_33,
        audio_34,
        audio_35,
        audio_36,
        audio_37,
        audio_38,
        audio_39,
        audio_40,
        audio_41,
        audio_42,
        audio_43,
        audio_44,
        audio_45,
        audio_46,
        audio_47,
        audio_48,
        audio_49,
        audio_50,
        audio_51,
        audio_52,
        audio_53
    }

    //SFX 종류들
    public enum ESfx
    {
        SFX_cutting,
        SFX_knifeGrab,
        SFX_garbageGrab,
        SFX_trashGrab,
        SFX_garbageDisable,
        SFX_trashDisable
    }

    //audio clip 담을 수 있는 배열
    [SerializeField] AudioClip[] narrations;
    [SerializeField] AudioClip[] sfxs;

    //플레이하는 AudioSource
    [SerializeField] AudioSource audioNarration;
    [SerializeField] AudioSource audioSfx;

    // ENarration 열거형을 매개변수로 받아 해당하는 나레이션 클립을 재생
    public void PlayNarrationE(ENarration narrationIdx)
    {
        //enum int형으로 형변환 가능
        audioNarration.clip = narrations[(int)narrationIdx];
        audioNarration.Play();

        //audioNarration.PlayOneShot(narrations[(int)narrationIdx]);
    }
    public void PlayNarration(int narrationIdx, AudioClip[] clips)
    {
        audioNarration.clip = clips[narrationIdx];
        audioNarration.Play();
    }
    public void PlayNarrationBuffer(AudioClip[] clips)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            PlayNarration(i, clips);
        }
    }
    public void PlayRandomNarration(AudioClip[] clips)
    {
        int r = Random.Range(0, clips.Length);
        audioNarration.PlayOneShot(clips[r]);
    }

    public void StopNarration()
    {
        audioNarration.Stop();
    }
    public bool IsPlaying()
    {
        return audioNarration.isPlaying;
    }

    //

    private Coroutine narrationCoroutine;
    public bool CEnd = true;

    public void PlayNarrationB(AudioClip[] clips, ref bool isOneShot)
    {
        

        // 이미 실행 중인 코루틴이 있으면 중단
        if (narrationCoroutine != null)
        {
            return;
        }
        isOneShot = true;
        CEnd = false;
        // 새로운 코루틴 시작
        narrationCoroutine = StartCoroutine(PlayClips(clips));
    }

    private IEnumerator PlayClips(AudioClip[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip == null) continue;

            audioNarration.clip = clip;
            audioNarration.Play();

            // 현재 클립이 끝날 때까지 대기
            yield return new WaitForSeconds(clip.length);

            yield return new WaitForSeconds(0.5f);
        }

        // 모든 클립 재생 완료 후 코루틴 종료
        narrationCoroutine = null;
        CEnd = true;
    }

    public void StopClips()
    {
        // 현재 재생 중지 및 코루틴 초기화
        if (narrationCoroutine != null)
        {
            StopCoroutine(narrationCoroutine);
            narrationCoroutine = null;
        }
        audioNarration.Stop();
    }

    // ESfx 열거형을 매개변수로 받아 해당하는 효과음 클립을 재생
    public void PlaySFX(ESfx esfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)esfx]);
    }

    //
    public AudioClip[] startScenes;
    public AudioClip[] pollutant;
    public AudioClip[] secondGarbage;
    public AudioClip[] secondGarbageCut;
    public AudioClip[] showHint;
    public AudioClip[] goodTrash;
    public AudioClip[] badTrash;
    public AudioClip[] finishGroup;

    //

    private void Start()
    {
        CEnd = true;
        //PlayNarrationB(startScenes);
    }

    public bool oneShotStart = false;
    public bool oneShot = false;
    public bool oneShotDone = false;
    public bool oneShotGarbageCut = false;
    public bool oneShotHint = false;

    private void Update()
    {
        if (GameManager.instance.isStart && !oneShotStart)
        {
            PlayNarrationB(startScenes, ref oneShotStart);
        }
        if (GameManager.instance.isGrab && !oneShot)
        {
            PlayNarrationB(secondGarbage, ref oneShot);
        }

        if (GameManager.instance.cutOnce && !oneShotGarbageCut)
        {
            PlayNarrationB(secondGarbageCut, ref oneShotGarbageCut);
        }
        if (oneShotGarbageCut && SoundManager.instance.CEnd && !oneShotHint)
        {
            GameManager.instance.HintOpen();

            PlayNarrationB(showHint, ref oneShotHint);
        }

        if (GameManager.instance.isDone >= 16 && !oneShotDone)
        {
            PlayNarrationB(finishGroup,     ref oneShotDone);
        }
    }


}
