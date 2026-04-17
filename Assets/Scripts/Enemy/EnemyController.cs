using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyMovement movement;
    private EnemyAnimation animationHandler;
    private EnemyAudio audioHandler;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
        animationHandler = GetComponent<EnemyAnimation>();
        audioHandler = GetComponent<EnemyAudio>();
    }

    void Update()
    {
        movement.HandleMovement();
        animationHandler.HandleAnimation();
        audioHandler.HandleAudio();
    }
}