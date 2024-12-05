using System.Collections;
using UnityEngine;

public class PlayAudioClips : MonoBehaviour
{
    public FadeScreen Fade;

    [System.Serializable]
    public struct ClipWithDelay
    {
        public AudioClip clip;               // 나레이션 오디오 클립
        public float delay;                  // 딜레이 (초 단위)
        public GameObject[] objectsToEnable; // 활성화할 오브젝트
        public GameObject[] objectsToDisable; // 비활성화할 오브젝트
    }

    public AudioSource narrationAudioSource; // 나레이션용 AudioSource
    public AudioSource backgroundAudioSource; // 배경음용 AudioSource
    public ClipWithDelay[] clipsWithDelays;   // 나레이션+Delay 배열

    private void Start()
    {
        if (narrationAudioSource == null || backgroundAudioSource == null)
        {
            Debug.LogError("AudioSource가 설정되지 않았음");
            return;
        }

        if (clipsWithDelays.Length == 0)
        {
            Debug.LogError("클립 배열이 비어 있음");
            return;
        }

        // 배경음 재생 시작
        if (!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.loop = true; // 반복 재생
            backgroundAudioSource.Play();
        }

        StartCoroutine(PlayClipsSequentially());
    }

    private IEnumerator PlayClipsSequentially()  // 나레이션 순차 재생 코루틴
    {
        foreach (var item in clipsWithDelays)
        {
            // 오디오 재생
            if (item.clip != null)
            {
                narrationAudioSource.clip = item.clip;
                narrationAudioSource.Play();
                yield return new WaitForSeconds(item.clip.length); // 나레이션 끝날 때까지 대기
            }


            // 변경이 있을 경우에만 페이드 효과
            bool anyStateChanged = AnyObjectStateChanged(item.objectsToEnable, true)
                                  || AnyObjectStateChanged(item.objectsToDisable, false);

            //여기에 페이드아웃
            if (anyStateChanged)
            {
                Fade.FadeOut();
                yield return new WaitForSeconds(Fade.fadeDuration); // 페이드 시간만큼 대기
            }

            // 오브젝트 상태 변경
            SetObjectStates(item.objectsToEnable, true);  // 활성화할 오브젝트들
            SetObjectStates(item.objectsToDisable, false); // 비활성화할 오브젝트들

            //여기에 페이드인
            if (anyStateChanged)
            {
                Fade.FadeIn(); // 페이드인
                yield return new WaitForSeconds(Fade.fadeDuration); // 페이드 시간만큼 대기
            }

            // 딜레이
            yield return new WaitForSeconds(item.delay);
        }
    }

    private void SetObjectStates(GameObject[] objects, bool state)  // 오브젝트 enable 상태 변경 함수
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
                return true; // 변경이 필요한 오브젝트가 있음
            }
        }

        return false; // 변경할 필요가 없음
    }
}

