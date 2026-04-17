using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    public float walkSpeed = 3.5f;
    public float runSpeed = 6.5f;
    private float currentSpeed;

    public Transform cameraTransform;
    public LayerMask groundLayer;

    private IceSurface currentIce;
    private Vector3 iceVelocity;
    private float iceExitTimer;
    private float iceExitDuration = 0.8f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = walkSpeed;
    }

    public void HandleSprint()
    {
        bool isRunning = Keyboard.current.leftShiftKey.isPressed;
        currentSpeed = isRunning ? runSpeed : walkSpeed; // Ternary operator
    }

    public Vector3 GetMovementInput(float x, float y)
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return (forward * y + right * x).normalized;
    }

    public void HandleMovement(Vector3 movement)
    {
        if (currentIce != null)
            HandleIceMovement(movement);
        else if (iceExitTimer > 0f)
            HandleIceExitMovement(movement);
        else
            HandleNormalMovement(movement);
    }

    void HandleIceMovement(Vector3 movement)
    {
        rb.AddForce(movement * currentSpeed, ForceMode.Acceleration);

        // LIMITE DE VELOCIDADE (ESSENCIAL)
        float maxIceSpeed = runSpeed * 1.5f;

        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (horizontalVel.magnitude > maxIceSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxIceSpeed;
            rb.linearVelocity = new Vector3(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);
        }

        if (rb.linearVelocity.magnitude < 0.1f)
            rb.linearVelocity = transform.forward * 0.3f;

        iceVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        iceExitTimer = iceExitDuration;
    }

    void HandleIceExitMovement(Vector3 movement)
    {
        iceExitTimer -= Time.fixedDeltaTime;
        float blend = iceExitTimer / iceExitDuration;

        Vector3 target = new Vector3(
            movement.x * currentSpeed,
            rb.linearVelocity.y,
            movement.z * currentSpeed
        );

        rb.linearVelocity = new Vector3(
            Mathf.Lerp(target.x, iceVelocity.x, blend),
            rb.linearVelocity.y,
            Mathf.Lerp(target.z, iceVelocity.z, blend)
        );
    }

    void HandleNormalMovement(Vector3 movement)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f, groundLayer))
        {
            Vector3 slope = Vector3.ProjectOnPlane(movement, hit.normal).normalized;

            rb.linearVelocity = new Vector3(
                slope.x * currentSpeed,
                rb.linearVelocity.y,
                slope.z * currentSpeed
            );
        }
        else
        {
            rb.linearVelocity = new Vector3(
                movement.x * currentSpeed,
                rb.linearVelocity.y,
                movement.z * currentSpeed
            );
        }
    }

    public void HandleRotation(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(movement);
            rb.MoveRotation(rot);
        }
    }

    public void SetIce(IceSurface ice)
    {
        currentIce = ice;
    }

    public float GetMaxSpeed()
    {
        return runSpeed;
    }

    public bool IsOnIce()
    {
        return currentIce != null;
    }
}