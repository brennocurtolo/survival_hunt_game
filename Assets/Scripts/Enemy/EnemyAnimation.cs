using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    private EnemyMovement movement;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<EnemyMovement>();
    }

    public void HandleAnimation()
    {
        Vector3 horizontalVelocity = new Vector3(
            movement.Velocity.x,
            0,
            movement.Velocity.z
        );

        float speed = horizontalVelocity.magnitude;
        float normalizedSpeed = Mathf.Clamp01(speed / movement.runSpeed);

        animator.SetFloat("Speed", normalizedSpeed, 0.1f, Time.deltaTime);
    }
}