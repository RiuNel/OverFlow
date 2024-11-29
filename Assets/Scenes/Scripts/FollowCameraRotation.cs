using UnityEngine;

public class FollowCameraPositionAndRotation : MonoBehaviour
{
    public Transform cameraTransform; // 따라갈 카메라의 Transform
    public Transform body;

    public Vector3 positionOffset;   // 오브젝트의 위치 오프셋

    void Update()
    {
        if (cameraTransform != null)
        {
            // 1. 위치 따라가기
            body.position = cameraTransform.position + positionOffset;

            // 2. Y축 회전만 따라가기
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
