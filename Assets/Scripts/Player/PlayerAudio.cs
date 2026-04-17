using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstep;

    private Animator animator;

    private PlayerMovement movement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    public void PlayFootstep()
    {
        if (animator.GetFloat("Speed") < 0.1f)
            return;

        if (movement.IsOnIce())
            return;
            
        audioSource.PlayOneShot(footstep);
    }
}