using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����̼� ���¸� ��Ÿ���� ������(enum)
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
    // �����̼� ���� �÷���
    public NarrationState currentNarrationState = NarrationState.None;

    // �����̼� ���¿� ���õ� ������Ʈ��
    public GameObject Truck; // MoveToStep ������Ʈ�� �����ϴ� ������Ʈ
    public GameObject Radio; // AudioSource�� �����ϴ� ������Ʈ

    private AudioSource audioSource; // ����� ����� ����ϴ� ������Ʈ
    public List<AudioClip> audioClips; // ����� ����� Ŭ�� ����Ʈ

    private int currentClipIndex = 0; // ���� ��� ���� Ŭ���� �ε���
    public bool isNarrationPlaying = false; // �����̼� ���� ����
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


        //TriggerFinishNarration();
        // ���� �����̼� Ʈ����
        CheckStartNarration();
    }

    // �����̼� ������ �����ϰ� ����
    private void CheckStartNarration()
    {
        // ���� �����̼��� ���� ���̸� �������� ����
        if (isNarrationPlaying) return;

        switch (currentNarrationState)
        {
            case NarrationState.Level1Start:
                PlayAudio(0); // audioClips�� 0�� �ε��� Ŭ�� ���
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
                Debug.Log("�����̼� ���� ���°� �������� �ʾҽ��ϴ�.");
                break;
        }
    }

    // �����̼� ���Ḧ Ʈ����
    //private void TriggerFinishNarration()
    //{
    //    // �����̼��� ���� ���̸� �������� ����
    //    if (isNarrationPlaying) return;

    //    switch (currentNarrationState)
    //    {

    //        default:
    //            Debug.Log("�����̼� ���� ���°� �������� �ʾҽ��ϴ�.");
    //            break;
    //    }

    //    currentNarrationState = NarrationState.None; // ���� �ʱ�ȭ
    //}

    // ��� �����̼� ���¿� �÷��׸� �ʱ�ȭ
    public void ResetAllNarration()
    {
        currentNarrationState = NarrationState.None; // �����̼� ���� �ʱ�ȭ
        isNarrationPlaying = false; // �����̼� ���� ���� �ʱ�ȭ
        hasTriggeredFinishNarration = false; // ���� �����̼� �ߺ� ���� �÷��� �ʱ�ȭ
    }

    // ������ ����� Ŭ���� ���
    void PlayAudio(int i)
    {
        isNarrationPlaying = true; //�ߺ� ���� ���� ����
        audioSource.clip = audioClips[i];
        audioSource.Play();
        StartCoroutine(WaitForAudio());
        Debug.Log("PlayAudio ����");
    }

    void PlayAudio(int i, int j, int k)
    {
        isNarrationPlaying = true; // �ߺ� ���� ���� ����
        StartCoroutine(PlaySequentialAudio(new int[] { i, j, k }));
    }

    // ���������� ������� ����ϴ� �ڷ�ƾ
    IEnumerator PlaySequentialAudio(int[] clipIndices)
    {
        foreach (int index in clipIndices)
        {
            // ���� ����� Ŭ�� ���� �� ���
            audioSource.clip = audioClips[index];
            audioSource.Play();

            // ���� ������� ���� ������ ���
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        ResetAllNarration(); // ��� �����̼� �÷��� �ʱ�ȭ
        isNarrationPlaying = false; // ���� ���� �ʱ�ȭ
        Debug.Log("��� ����� ��� �Ϸ�");
    }

    // ���� ����� Ŭ���� ���� �� �����̼� ���� �ʱ�ȭ
    IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        //isNarrationPlaying = false;
        ResetAllNarration(); // ��� �����̼� �÷��� �ʱ�ȭ
        Debug.Log("WaitForAudio ����");
    }
    
}
