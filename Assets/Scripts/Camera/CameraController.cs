using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float distance = 5f;
    public float mouseSensitivity = 100f;

    public float minY = -20f;
    public float maxY = 60f;

    public LayerMask collisionMask;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private Vector3 velocity;
    private Vector3 currentPivot;

    private float currentDistance;

    private Rigidbody playerRb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        currentPivot = player.position + Vector3.up * 1.5f;
        currentDistance = distance;

        playerRb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // ===== VELOCIDADE DO PLAYER =====
        float playerSpeed = playerRb != null ? playerRb.linearVelocity.magnitude : 0f;

        // ===== PIVOT =====
        Vector3 targetPivot = player.position + Vector3.up * 1.5f;

        // MAIS RÁPIDO (melhor pra gelo)
        currentPivot = Vector3.Lerp(currentPivot, targetPivot, 25f * Time.deltaTime);

        // ===== INPUT =====
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX += mouseX;
        rotationY -= mouseY;

        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // ===== ROTAÇÃO =====
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // ===== DISTÂNCIA DINÂMICA =====
        float dynamicDistance = distance + Mathf.Clamp(playerSpeed * 0.2f, 0f, 2f);

        Vector3 offset = rotation * new Vector3(0, 0, -dynamicDistance);

        float cameraRadius = 0.3f;
        RaycastHit hit;

        float targetDistance = dynamicDistance;

        // ===== COLISÃO =====
        if (Physics.SphereCast(currentPivot, cameraRadius, offset.normalized, out hit, dynamicDistance, collisionMask))
        {
            targetDistance = hit.distance - 0.2f;
        }

        // ===== SUAVIZA DISTÂNCIA =====
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, 10f * Time.deltaTime);

        // PROTEÇÃO
        float minDistanceFromPlayer = 1.5f;
        currentDistance = Mathf.Max(currentDistance, minDistanceFromPlayer);

        // ===== POSIÇÃO FINAL =====
        Vector3 finalPosition = currentPivot + offset.normalized * currentDistance;

        // ===== SMOOTH ADAPTATIVO =====
        float distanceToTarget = Vector3.Distance(transform.position, finalPosition);

        float speedFactor = Mathf.Clamp(playerSpeed / 8f, 0.5f, 2f);

        float smoothTime = distanceToTarget > 2f ? 0.01f / speedFactor : 0.05f / speedFactor;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            finalPosition,
            ref velocity,
            smoothTime
        );

        // ===== LOOK =====
        transform.LookAt(currentPivot);
    }
}