using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class knifeCtr : MonoBehaviour
{
    public Transform knifePos;
    public bool isKnife; //Į ������̳�
    public bool isKnifePos; //Į�� ���ڸ��� �ֳ�

    public GameObject cutObject;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        transform.position = knifePos.position;
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        // selectEntered �̺�Ʈ�� ������ �߰�
        grabInteractable.selectEntered.AddListener(OnGrabBegin);
        grabInteractable.selectExited.AddListener(OnGrabEnd);
    }

    private void OnDisable()
    {
        // �̺�Ʈ ������ ����
        grabInteractable.selectEntered.RemoveListener(OnGrabBegin);
        grabInteractable.selectExited.RemoveListener(OnGrabEnd);
    }


    private void OnGrabBegin(SelectEnterEventArgs args)
    {
        Debug.Log($"{gameObject.name}�� �������ϴ�.");
        isKnife = true;
        isKnifePos = false;
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        Debug.Log($"{gameObject.name}�� �������ϴ�.");
        isKnife = false;
    }

    private void Update()
    {
        ReturnKnife();
    }

    void ReturnKnife()
    {
        //���� ������� �ƴϰ� �ڸ��� ���ٸ� 5�� �ڿ� ����ġ��
        if (knifePos != null && !isKnife && !isKnifePos)
        {
            Invoke("ReturnKnifeAfterDelay", 5f);
        }
    }

    void ReturnKnifeAfterDelay()
    {
        gameObject.SetActive(false);          // Į ��Ȱ��ȭ
        transform.position = knifePos.position;
        isKnife = false;
        isKnifePos = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "trash")
        {
            cutObject.SetActive(true);
        }
    }
}
