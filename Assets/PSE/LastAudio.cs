using System.Collections;
using UnityEngine;

public class PlayAudioClips : MonoBehaviour
{
    public FadeScreen Fade;

    [System.Serializable]
    public struct ClipWithDelay
    {
        public AudioClip clip;               // �����̼� ����� Ŭ��
        public float delay;                  // ������ (�� ����)
        public GameObject[] objectsToEnable; // Ȱ��ȭ�� ������Ʈ
        public GameObject[] objectsToDisable; // ��Ȱ��ȭ�� ������Ʈ
    }

    public AudioSource narrationAudioSource; // �����̼ǿ� AudioSource
    public AudioSource backgroundAudioSource; // ������� AudioSource
    public ClipWithDelay[] clipsWithDelays;   // �����̼�+Delay �迭

    private void Start()
    {
        if (narrationAudioSource == null || backgroundAudioSource == null)
        {
            Debug.LogError("AudioSource�� �������� �ʾ���");
            return;
        }

        if (clipsWithDelays.Length == 0)
        {
            Debug.LogError("Ŭ�� �迭�� ��� ����");
            return;
        }

        // ����� ��� ����
        if (!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.loop = true; // �ݺ� ���
            backgroundAudioSource.Play();
        }

        StartCoroutine(PlayClipsSequentially());
    }

    private IEnumerator PlayClipsSequentially()  // �����̼� ���� ��� �ڷ�ƾ
    {
        foreach (var item in clipsWithDelays)
        {
            // ����� ���
            if (item.clip != null)
            {
                narrationAudioSource.clip = item.clip;
                narrationAudioSource.Play();
                yield return new WaitForSeconds(item.clip.length); // �����̼� ���� ������ ���
            }


            // ������ ���� ��쿡�� ���̵� ȿ��
            bool anyStateChanged = AnyObjectStateChanged(item.objectsToEnable, true)
                                  || AnyObjectStateChanged(item.objectsToDisable, false);

            //���⿡ ���̵�ƿ�
            if (anyStateChanged)
            {
                Fade.FadeOut();
                yield return new WaitForSeconds(Fade.fadeDuration); // ���̵� �ð���ŭ ���
            }

            // ������Ʈ ���� ����
            SetObjectStates(item.objectsToEnable, true);  // Ȱ��ȭ�� ������Ʈ��
            SetObjectStates(item.objectsToDisable, false); // ��Ȱ��ȭ�� ������Ʈ��

            //���⿡ ���̵���
            if (anyStateChanged)
            {
                Fade.FadeIn(); // ���̵���
                yield return new WaitForSeconds(Fade.fadeDuration); // ���̵� �ð���ŭ ���
            }

            // ������
            yield return new WaitForSeconds(item.delay);
        }
    }

    private void SetObjectStates(GameObject[] objects, bool state)  // ������Ʈ enable ���� ���� �Լ�
    {
        if (objects == null) return;

        foreach (var obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }

    private bool AnyObjectStateChanged(GameObject[] objects, bool desiredState)
    {
        if (objects == null) return false;

        foreach (var obj in objects)
        {
            if (obj != null && obj.activeSelf != desiredState)
            {
                return true; // ������ �ʿ��� ������Ʈ�� ����
            }
        }

        return false; // ������ �ʿ䰡 ����
    }
}

