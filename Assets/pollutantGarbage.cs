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
    public float moveSpeed = 0.1f;    // �̵� �ӵ�

    


    public float fadeDuration = 2.0f; // ���������� �� �ɸ��� �ð�
    private Material material;
    private Color originalColor;
    private float fadeTimer;

    void Start()
    {
        // Renderer�� ��Ƽ���� ��������
        Renderer renderer = pollutionImage.GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material; // Renderer�� �ν��Ͻ� ��Ƽ���� ��������
            originalColor = material.color; // �ʱ� ���� ����
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
        // 1. pollutantComimg ȣ�� �� �Ϸ� ���
        yield return StartCoroutine(PollutantComing());

        // 2. pollutantOut ȣ��
        PollutantOut();

        // 3. 0.5�� ��� (�ʿ�� �ð� ����)
        yield return new WaitForSeconds(0.5f);

        // 4. pollutantFin ȣ��
        PollutantFin();
    }

    private IEnumerator PollutantComing()
    {
        // ��ǥ ��ġ�� ������ ������ MoveTowards
        while (Vector3.Distance(pollution.transform.position, pollutionPos.position) > 0.01f)
        {
            pollution.transform.position = Vector3.MoveTowards(
                pollution.transform.position,
                pollutionPos.position,
                speed * Time.deltaTime
            );
            yield return null; // ���� �����ӱ��� ���
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

            // ���İ� ���
            float alpha = Mathf.Lerp(originalColor.a, 0, fadeTimer / fadeDuration);

            // ��Ƽ���� ���� ������Ʈ
            Color newColor = originalColor;
            newColor.a = alpha;
            material.color = newColor;

            // ������ ������������ ������Ʈ ��Ȱ��ȭ
            if (fadeTimer >= fadeDuration)
            {
                pollutionImage.SetActive(false);
            }
        }
    }
}
