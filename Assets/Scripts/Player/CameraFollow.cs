using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player object
    public float smoothSpeed = 5f; // Smoothing factor for movement
    public float fixedY = 30f; // Fixed Y position of the camera

    private Vector2 xLimits = new Vector2(30f, 100f);
    private Vector2 zLimits = new Vector2(-60f, -15f);

    void LateUpdate()
    {
        if (player == null) return;

        // Target position based on player
        Vector3 targetPosition = new Vector3(player.position.x, fixedY, player.position.z);

        // Clamp the X and Z positions
        targetPosition.x = Mathf.Clamp(targetPosition.x, xLimits.x, xLimits.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, zLimits.x, zLimits.y);

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
