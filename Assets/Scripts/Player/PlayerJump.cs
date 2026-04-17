using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;

    public float jumpForce = 6f;
    public LayerMask groundLayer;

    private bool isGrounded = true;

    private PlayerAnimation animationController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animationController = GetComponent<PlayerAnimation>();
    }

    public void Jump()
    {
        if (!isGrounded) return;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isGrounded = false;
        animationController.TriggerJump();
    }

    public void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 1.1f, groundLayer))
            isGrounded = hit.distance <= 0.5f; // Try different values for better feel
        else
            isGrounded = false;
    }

    public bool IsGrounded() => isGrounded;

    public float GetVerticalVelocity() => rb.linearVelocity.y;
}