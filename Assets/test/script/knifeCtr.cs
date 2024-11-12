using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class knifeCtr : MonoBehaviour
{
    public Transform knifePos;
    public bool isKnife=true; //Į ������̳�
    public bool isKnifePos=true; //Į�� ���ڸ��� �ֳ�

    
    public GameObject table;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        isKnife = true;
        isKnifePos  = true;
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
        //Debug.Log($"{gameObject.name}�� �������ϴ�.");
        table.SetActive(false);
        isKnife = true;
        isKnifePos = false;
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        //Debug.Log($"{gameObject.name}�� �������ϴ�.");
        table.SetActive(true);
        isKnife = false;
    }

    private void Update()
    {
        ReturnKnife();
    }

    void ReturnKnife()
    {
        //���� ������� �ƴϰ� �ڸ��� ���ٸ� ����ġ��
        if (knifePos != null && !isKnife && !isKnifePos)
        {
            Invoke("ReturnKnifeAfterDelay", 2f);
        }
    }

    void ReturnKnifeAfterDelay()
    {
        gameObject.SetActive(false);          // Į ��Ȱ��ȭ
        transform.position = knifePos.position;
        isKnife = false;
        isKnifePos = true;
        gameObject.SetActive(true);
    }
}
