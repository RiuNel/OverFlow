using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;  // Transform of the main VR camera
    public Transform portal;        // Transform of the window or portal object
    public Transform otherWorldPortal; // Transform of the corresponding portal in the other world
    public Camera otherWorldCamera; // Camera in the alternate world

    void LateUpdate()
    {
        // Calculate the offset from the player to the portal
        Vector3 playerOffsetFromPortal = playerCamera.position - portal.position;

        // Position the other world camera relative to the corresponding portal in the other world
        otherWorldCamera.transform.position = otherWorldPortal.position + playerOffsetFromPortal;

        // Calculate the angular difference between the portal's rotation and the player's camera rotation
        Quaternion portalRotationDifference = portal.rotation * Quaternion.Inverse(playerCamera.rotation);
        
        // Apply the rotational difference to the other world camera
        otherWorldCamera.transform.rotation = portalRotationDifference * otherWorldPortal.rotation;
    }
}
