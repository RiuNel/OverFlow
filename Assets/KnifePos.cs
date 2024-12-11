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
            // ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
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
            // �ڷ�ƾ�� ���� ���� �ƴϸ� ���� ����
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

        // �ڷ�ƾ ���� ó��
        returnKnifeCoroutine = null;
    }
}
