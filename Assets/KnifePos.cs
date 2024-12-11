using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KnifePos : MonoBehaviour
{
    public GameManager GameManager;
    public Transform knifePos;

    public bool isKnife;
    public bool isKnifePos;

    private Coroutine returnKnifeCoroutine;


    private void Awake()
    {
        isKnife = false;
        isKnifePos = true;
    }

    private void Update()
    {
        if (GameManager.isGrab)
        {
            // 실행 중인 코루틴이 있다면 중단
            if (returnKnifeCoroutine != null)
            {
                StopCoroutine(returnKnifeCoroutine);
                returnKnifeCoroutine = null;
            }
            isKnife = true;
            isKnifePos = false;
        }

        if(!GameManager.isGrab && isKnife && returnKnifeCoroutine == null)
        {
            // 코루틴이 실행 중이 아니면 새로 시작
            returnKnifeCoroutine = StartCoroutine(ReturnKnifeCoroutine());
        }

        
    }
    IEnumerator ReturnKnifeCoroutine()
    {
        yield return new WaitForSeconds(5f);
        ReturnKnife();
    }

    void ReturnKnife()
    {
        transform.position = knifePos.position;
        isKnife = false;
        isKnifePos = true;

        // 코루틴 종료 처리
        returnKnifeCoroutine = null;
    }
}
