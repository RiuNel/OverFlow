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

    //BGM ������
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

    //SFX ������
    public enum ESfx
    {
        SFX_cutting,
        SFX_knifeGrab,
        SFX_garbageGrab,
        SFX_trashGrab,
        SFX_garbageDisable,
        SFX_trashDisable
    }

    //audio clip ���� �� �ִ� �迭
    [SerializeField] AudioClip[] narrations;
    [SerializeField] AudioClip[] sfxs;

    //�÷����ϴ� AudioSource
    [SerializeField] AudioSource audioNarration;
    [SerializeField] AudioSource audioSfx;

    // ENarration �������� �Ű������� �޾� �ش��ϴ� �����̼� Ŭ���� ���
    public void PlayNarrationE(ENarration narrationIdx)
    {
        //enum int������ ����ȯ ����
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
        

        // �̹� ���� ���� �ڷ�ƾ�� ������ �ߴ�
        if (narrationCoroutine != null)
        {
            return;
        }
        isOneShot = true;
        CEnd = false;
        // ���ο� �ڷ�ƾ ����
        narrationCoroutine = StartCoroutine(PlayClips(clips));
    }

    private IEnumerator PlayClips(AudioClip[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip == null) continue;

            audioNarration.clip = clip;
            audioNarration.Play();

            // ���� Ŭ���� ���� ������ ���
            yield return new WaitForSeconds(clip.length);

            yield return new WaitForSeconds(0.5f);
        }

        // ��� Ŭ�� ��� �Ϸ� �� �ڷ�ƾ ����
        narrationCoroutine = null;
        CEnd = true;
    }

    public void StopClips()
    {
        // ���� ��� ���� �� �ڷ�ƾ �ʱ�ȭ
        if (narrationCoroutine != null)
        {
            StopCoroutine(narrationCoroutine);
            narrationCoroutine = null;
        }
        audioNarration.Stop();
    }

    // ESfx �������� �Ű������� �޾� �ش��ϴ� ȿ���� Ŭ���� ���
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
