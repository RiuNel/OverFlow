using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class pollutantGarbage : MonoBehaviour
{
    public GameObject pollution;
    public GameObject pollutionImage;

    public Transform pollutionPos;

    public float speed;
    public float moveSpeed = 0.1f;    // 이동 속도

    


    public float fadeDuration = 2.0f; // 투명해지는 데 걸리는 시간
    private Material material;
    private Color originalColor;
    private float fadeTimer;

    void Start()
    {
        // Renderer와 머티리얼 가져오기
        Renderer renderer = pollutionImage.GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // Renderer의 인스턴스 머티리얼 가져오기
            originalColor = material.color; // 초기 색상 저장
        }
    }


    private void OnEnable()
    {
        pollution.SetActive(true);
        pollutionImage.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.instance.pollutionPlay)
        {
            StartCoroutine(PollutionSequence());
        }
    }

    private IEnumerator PollutionSequence()
    {
        // 1. pollutantComimg 호출 및 완료 대기
        yield return StartCoroutine(PollutantComing());

        // 2. pollutantOut 호출
        PollutantOut();

        // 3. 0.5초 대기 (필요시 시간 조정)
        yield return new WaitForSeconds(0.5f);

        // 4. pollutantFin 호출
        PollutantFin();
    }

    private IEnumerator PollutantComing()
    {
        // 목표 위치에 도달할 때까지 MoveTowards
        while (Vector3.Distance(pollution.transform.position, pollutionPos.position) > 0.01f)
        {
            pollution.transform.position = Vector3.MoveTowards(
                pollution.transform.position,
                pollutionPos.position,
                speed * Time.deltaTime
            );
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void PollutantOut()
    {
        pollution.SetActive(false);
        pollutionImage.SetActive(true);
    }
    public void PollutantFin()
    {
        pollutionImage.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (material != null && fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;

            // 알파값 계산
            float alpha = Mathf.Lerp(originalColor.a, 0, fadeTimer / fadeDuration);

            // 머티리얼 색상 업데이트
            Color newColor = originalColor;
            newColor.a = alpha;
            material.color = newColor;

            // 완전히 투명해졌으면 오브젝트 비활성화
            if (fadeTimer >= fadeDuration)
            {
                pollutionImage.SetActive(false);
            }
        }
    }
}
