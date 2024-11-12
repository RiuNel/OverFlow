using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class knifeCtr : MonoBehaviour
{
    public Transform knifePos;
    public bool isKnife=true; //칼 사용중이냐
    public bool isKnifePos=true; //칼이 제자리에 있냐

    
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
        // selectEntered 이벤트에 리스너 추가
        grabInteractable.selectEntered.AddListener(OnGrabBegin);
        grabInteractable.selectExited.AddListener(OnGrabEnd);
    }

    private void OnDisable()
    {
        // 이벤트 리스너 제거
        grabInteractable.selectEntered.RemoveListener(OnGrabBegin);
        grabInteractable.selectExited.RemoveListener(OnGrabEnd);
    }


    private void OnGrabBegin(SelectEnterEventArgs args)
    {
        //Debug.Log($"{gameObject.name}가 잡혔습니다.");
        table.SetActive(false);
        isKnife = true;
        isKnifePos = false;
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        //Debug.Log($"{gameObject.name}가 놓였습니다.");
        table.SetActive(true);
        isKnife = false;
    }

    private void Update()
    {
        ReturnKnife();
    }

    void ReturnKnife()
    {
        //낫이 사용중이 아니고 자리에 없다면 원위치로
        if (knifePos != null && !isKnife && !isKnifePos)
        {
            Invoke("ReturnKnifeAfterDelay", 2f);
        }
    }

    void ReturnKnifeAfterDelay()
    {
        gameObject.SetActive(false);          // 칼 비활성화
        transform.position = knifePos.position;
        isKnife = false;
        isKnifePos = true;
        gameObject.SetActive(true);
    }
}
