using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastAudio : MonoBehaviour
{
    [System.Serializable]
    public struct ClipWithDelay
    {
        public AudioClip clip;    // ����� Ŭ��
        public float delay;       // ���� Ŭ�������� ��
    }

    public AudioSource audioSource;      // ������� ����� AudioSource
    public ClipWithDelay[] clipsWithDelays; // ����� Ŭ���� �� ���� �迭

    private void Start()
    {
        StartCoroutine(PlayClipsSequentially());
    }

    private IEnumerator PlayClipsSequentially()
    {
        foreach (var item in clipsWithDelays)
        {
            audioSource.clip = item.clip; // AudioSource�� Ŭ�� ����
            audioSource.Play();           // Ŭ�� ���
            yield return new WaitForSeconds(item.clip.length + item.delay);
        }
    }
}
