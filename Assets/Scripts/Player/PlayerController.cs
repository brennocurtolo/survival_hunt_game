using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerJump jump;
    private PlayerAnimation animationController;

    private float movementX;
    private float movementY;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        jump = GetComponent<PlayerJump>();
        animationController = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        movement.HandleSprint();
        jump.CheckGround();
        animationController.UpdateAnimation();
    }

    void FixedUpdate()
    {
        Vector3 move = movement.GetMovementInput(movementX, movementY);
        movement.HandleMovement(move);
        movement.HandleRotation(move);
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        movementX = input.x;
        movementY = input.y;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            jump.Jump();
    }
}