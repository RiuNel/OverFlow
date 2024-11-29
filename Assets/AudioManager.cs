using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; // Audio Source 연결
    public AudioClip narrationClip; // 나레이션 클립
    public AudioClip effectClip;    // 효과음 클립

    void Start()
    {
        // 시작 시 나레이션 재생
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
