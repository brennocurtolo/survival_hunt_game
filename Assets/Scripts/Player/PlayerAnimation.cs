using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerJump jump;
    private Rigidbody rb;
    private PlayerMovement movement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        jump = GetComponent<PlayerJump>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    public void UpdateAnimation()
    {
        float speed = rb.linearVelocity.magnitude;
        float normalizedSpeed = Mathf.InverseLerp(0, movement.GetMaxSpeed(), speed);

        animator.SetFloat("Speed", normalizedSpeed, 0.1f, Time.deltaTime);

        float yVel = rb.linearVelocity.y;

        animator.SetBool("IsFalling", !jump.IsGrounded() && yVel < 0);
        animator.SetBool("IsJumping", !jump.IsGrounded() && yVel > 0.2f);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
}