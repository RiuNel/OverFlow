using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Audio Source ����
    public AudioClip narrationClip; // �����̼� Ŭ��
    public AudioClip effectClip;    // ȿ���� Ŭ��

    void Start()
    {
        // ���� �� �����̼� ���
        PlayNarration();
    }

    public void PlayNarration()
    {
        audioSource.clip = narrationClip;
        audioSource.Play();
    }

    public void PlayEffect()
    {
        audioSource.clip = effectClip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
