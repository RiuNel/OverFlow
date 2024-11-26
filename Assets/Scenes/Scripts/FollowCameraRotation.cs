using UnityEngine;

public class FollowCameraPositionAndRotation : MonoBehaviour
{
    public Transform cameraTransform; // ���� ī�޶��� Transform
    public Transform body;

    public Vector3 positionOffset;   // ������Ʈ�� ��ġ ������

    void Update()
    {
        if (cameraTransform != null)
        {
            // 1. ��ġ ���󰡱�
            body.position = cameraTransform.position + positionOffset;

            // 2. Y�� ȸ���� ���󰡱�
            Vector3 targetRotation = body.eulerAngles;
            targetRotation.y = cameraTransform.eulerAngles.y;
            body.eulerAngles = targetRotation;
        }

        if(body.position.y < 0)
        {
            body.position = new Vector3(body.position.x, 0, body.position.z);
        }
}
}
