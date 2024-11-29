using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationControl : MonoBehaviour
{
    // �����̼� ���� ���� �÷���
    public bool level1_start_narration = false;
    public bool level1_stop_narration = false;
    public bool level2_start_narration = false;
    public bool level2_stop_narration = false;
    public bool level3_start_narration = false; 
    public bool level3_stop_narration = false;

    // �����̼� ���� ���� �÷���
    public bool level1_finish_narration = false;
    public bool level2_finish_narration = false;
    public bool level3_finish_narration = false;

    // �����̼��� ���� ���� ������ ����
    public bool isNarrationPlaying = false;

    // �����̼� ���¿� ���õ� ������Ʈ��
    public GameObject Truck; // MoveToStep ������Ʈ�� �����ϴ� ������Ʈ
    public GameObject Radio; // AudioSource�� �����ϴ� ������Ʈ

    private AudioSource audioSource; // ����� ����� ����ϴ� ������Ʈ
    public List<AudioClip> audioClips; // ����� ����� Ŭ�� ����Ʈ

    private int currentClipIndex = 0; // ���� ��� ���� Ŭ���� �ε���
    private bool hasTriggeredFinishNarration = false; // ���� �����̼� �ߺ� ���� ����

    private void Start()
    {
        // Radio�� �����Ǿ����� Ȯ��
        if (Radio == null)
        {
            Debug.LogError("Radio�� �Ҵ���� �ʾҽ��ϴ�. Unity �ν����Ϳ��� �����ϼ���.");
            return;
        }

        // Radio���� AudioSource ������Ʈ�� ������
        audioSource = Radio.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Radio ��ü�� AudioSource ������Ʈ�� �����ϴ�. AudioSource�� �߰��ϼ���.");
        }

        // Truck ������Ʈ�� �����Ǿ����� Ȯ��
        if (Truck == null)
        {
            Debug.LogError("Truck ��ü�� �Ҵ���� �ʾҽ��ϴ�. Unity �ν����Ϳ��� �����ϼ���.");
        }
    }

    void Update()
    {
        // Truck���� MoveToStep ������Ʈ ��������
        var moveToStep = Truck?.GetComponent<MoveToStep>();
        if (moveToStep == null)
        {
            Debug.LogError("Truck�� MoveToStep ������Ʈ�� �����ϴ�. �߰��ϰų� ������Ʈ�� Ȯ���ϼ���.");
            return;
        }

        // ���� �����̼� Ʈ����: finishStop ���°� true�� �� ����
        if (moveToStep.finishStop && !hasTriggeredFinishNarration)
        {
            TriggerFinishNarration(); // ���� �����̼� ����
            hasTriggeredFinishNarration = true; // �ߺ� ���� ����
        }

        // ���� �����̼� Ʈ����
        CheckStartNarration();
    }

    // �����̼� ������ �����ϰ� ����
    private void CheckStartNarration()
    {
        // ���� �����̼��� ���� ���̸� �������� ����
        if (isNarrationPlaying) return;

        // �����̼� ���� �÷��׿� ���� ������ �����̼� ����
        if (level1_start_narration)
        {
            PlayAudio(0); // audioClips�� 0�� �ε��� Ŭ�� ���
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
            Debug.Log("�����̼� ���� �÷��װ� �������� �ʾҽ��ϴ�.");
        }
        


        
    }

    // �����̼� ���Ḧ Ʈ����
    private void TriggerFinishNarration()
    {
        // �����̼��� ���� ���̸� �������� ����
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
            Debug.Log("�����̼� ���� �÷��װ� �������� �ʾҽ��ϴ�.");
        }
    }

    // ��� �����̼� ���¿� �÷��׸� �ʱ�ȭ
    public void ResetAllNarration()
    {
        // ���� �÷��� �ʱ�ȭ
        level1_start_narration = false;
        level1_stop_narration = false;
        level2_start_narration = false;
        level3_start_narration = false;

        // ���� �÷��� �ʱ�ȭ
        level1_finish_narration = false;
        level2_finish_narration = false;
        level3_finish_narration = false;

        isNarrationPlaying = false; // �����̼� ���� �ʱ�ȭ
        hasTriggeredFinishNarration = false; // ���� �����̼� �ߺ� ���� �÷��� �ʱ�ȭ
    }

    // ������ ������ ����� Ŭ���� ���
    void PlayAudio(int min, int max)
    {
        isNarrationPlaying = true;
        // audioClips �� ���� ��ȿ�� �˻�
        if (audioClips == null || audioClips.Count == 0 || min < 0 || max >= audioClips.Count)
        {
            Debug.LogWarning("����� Ŭ�� ����Ʈ�� ��� �ְų� �ε��� ������ �߸��Ǿ����ϴ�.");
            return;
        }

        // ���� Ŭ�� �ε��� ����
        currentClipIndex = min;
        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();

        // ���� Ŭ���� ������ ���� Ŭ�� ���
        StartCoroutine(WaitForAudioToEnd(min, max));
    }

    void PlayAudio(int i)
    {
        isNarrationPlaying = true;
        audioSource.clip = audioClips[i];
        audioSource.Play();
        StartCoroutine(WaitForAudio());
    }

    // ���� ����� Ŭ���� ���� �� ���� Ŭ������ �Ѿ
    IEnumerator WaitForAudioToEnd(int min, int max)
    {
        // ���� Ŭ���� ���̸�ŭ ���
        yield return new WaitForSeconds(audioSource.clip.length);

        // ���� Ŭ������ �̵�
        currentClipIndex++;
        
        if (currentClipIndex <= max)
        {
            PlayAudio(currentClipIndex, max); // ��� ȣ��� ���� Ŭ�� ���
        }
        else
        {
            Debug.Log("�� �����̼�");
            isNarrationPlaying = false;
            ResetAllNarration(); // ��� �����̼� �÷��� �ʱ�ȭ
        }
    }
    IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        isNarrationPlaying = false;
        ResetAllNarration(); // ��� �����̼� �÷��� �ʱ�ȭ
    }
}
