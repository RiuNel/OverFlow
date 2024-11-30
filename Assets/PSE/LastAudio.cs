using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastAudio : MonoBehaviour
{
    [System.Serializable]
    public struct ClipWithDelay
    {
        public AudioClip clip;    // 오디오 클립
        public float delay;       // 다음 클립까지의 텀
    }

    public AudioSource audioSource;      // 오디오를 재생할 AudioSource
    public ClipWithDelay[] clipsWithDelays; // 오디오 클립과 텀 정보 배열

    private void Start()
    {
        StartCoroutine(PlayClipsSequentially());
    }

    private IEnumerator PlayClipsSequentially()
    {
        foreach (var item in clipsWithDelays)
        {
            audioSource.clip = item.clip; // AudioSource에 클립 설정
            audioSource.Play();           // 클립 재생
            yield return new WaitForSeconds(item.clip.length + item.delay);
        }
    }
}
