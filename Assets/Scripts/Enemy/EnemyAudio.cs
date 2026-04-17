using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private EnemyMovement movement;

    [Header("Audio")]
    public AudioClip chaseClip;

    [Header("Volume Settings")]
    public float maxVolume = 0.5f;
    public float minVolume = 0f;
    public float fadeSpeed = 2f;

    private static float closestDistance = Mathf.Infinity;
    private static float currentDetectionDistance;
    private static bool hasEnemyChasing;
    private static int lastFrameUpdated = -1;

    void Start()
    {
        movement = GetComponent<EnemyMovement>();
    }

    public void HandleAudio()
    {
        if (lastFrameUpdated != Time.frameCount)
        {
            closestDistance = Mathf.Infinity;
            hasEnemyChasing = false;
            lastFrameUpdated = Time.frameCount;
        }

        if (movement.IsChasing)
        {
            hasEnemyChasing = true;

            float distance = movement.DistanceToPlayer;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentDetectionDistance = movement.DetectionDistance;
            }
        }
    }

    void LateUpdate()
    {
        ApplyAudio();
    }

    void ApplyAudio()
    {
        if (hasEnemyChasing)
        {
            AudioManager.Instance.PlayLoop(chaseClip);

            float t = Mathf.Clamp01(1 - (closestDistance / currentDetectionDistance));
            float targetVolume = Mathf.Lerp(minVolume, maxVolume, t);

            float newVolume = Mathf.MoveTowards(
                AudioManager.Instance.musicSource.volume,
                targetVolume,
                fadeSpeed * Time.deltaTime
            );

            AudioManager.Instance.SetVolume(newVolume);
        }
        else
        {
            float newVolume = Mathf.MoveTowards(
                AudioManager.Instance.musicSource.volume,
                0f,
                fadeSpeed * Time.deltaTime
            );

            AudioManager.Instance.SetVolume(newVolume);

            if (newVolume <= 0.01f)
            {
                AudioManager.Instance.StopLoop();
            }
        }
    }
}