using System.Collections;
using UnityEngine;

public class NarrationControl : MonoBehaviour
{
    public bool level1_start_narration = false;
    public bool level2_start_narration = false;
    public bool level3_start_narration = false;

    public bool level1_finish_narration = false;
    public bool level2_finish_narration = false;
    public bool level3_finish_narration = false;

    public bool isNarrationPlaying = false; // �����̼��� ���� ������ Ȯ��

    public GameObject Truck;

    private bool hasTriggeredFinishNarration = false; // �ߺ� ���� ����

    void Update()
    {
        var moveToStep = Truck.GetComponent<MoveToStep>(); // finishStop ���� Ȯ��

        if (moveToStep == null)
        {
            Debug.LogError("MoveToStep ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // finishStop�� true�� �� ���� �����̼� ����
        if (moveToStep.finishStop && !hasTriggeredFinishNarration)
        {
            TriggerFinishNarration(); // ���� �����̼� ����
            hasTriggeredFinishNarration = true; // �ߺ� ���� ����
        }

        CheckStartNarration();
    }

    private void CheckStartNarration()
    {
        if (isNarrationPlaying) return; // �����̼� ���̸� �ߺ� ���� ����

        switch (true)
        {
            case var _ when level1_start_narration:
                StartCoroutine(StartNarration("Level 1 Start Narration", 5f)); // 5�� �����̼�
                break;
            case var _ when level2_start_narration:
                StartCoroutine(StartNarration("Level 2 Start Narration", 7f)); // 7�� �����̼�
                break;
            case var _ when level3_start_narration:
                StartCoroutine(StartNarration("Level 3 Start Narration", 10f)); // 10�� �����̼�
                break;
            default:
                Debug.Log("No start narration active.");
                break;
        }
    }

    private void TriggerFinishNarration()
    {
        if (isNarrationPlaying) return; // �����̼� ���̸� ���� �� ��

        switch (true)
        {
            case var _ when level1_finish_narration:
                StartCoroutine(StartNarration("Level 1 Finish Narration", 4f)); // 4�� ���� �����̼�
                break;
            case var _ when level2_finish_narration:
                StartCoroutine(StartNarration("Level 2 Finish Narration", 6f)); // 6�� ���� �����̼�
                break;
            case var _ when level3_finish_narration:
                StartCoroutine(StartNarration("Level 3 Finish Narration", 8f)); // 8�� ���� �����̼�
                break;
            default:
                Debug.Log("No finish narration active.");
                break;
        }
    }

    private IEnumerator StartNarration(string narrationMessage, float duration)
    {
        Debug.Log($"Starting: {narrationMessage}");
        isNarrationPlaying = true;

        // �����̼� ���� �� �۾�
        yield return new WaitForSeconds(duration); // �����̼� ���� �ð� ��ٸ���

        Debug.Log($"Ending: {narrationMessage}");
        ResetAllNarration(); // �����̼� ���� �� ���� �ʱ�ȭ
    }

    private void ResetAllNarration()
    {
        level1_start_narration = false;
        level2_start_narration = false;
        level3_start_narration = false;

        level1_finish_narration = false;
        level2_finish_narration = false;
        level3_finish_narration = false;

        isNarrationPlaying = false; // �����̼� ���� �ʱ�ȭ
        hasTriggeredFinishNarration = false; // ���� �����̼� Ʈ���� �ʱ�ȭ
    }
}
