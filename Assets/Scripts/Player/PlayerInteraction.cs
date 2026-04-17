using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerStats stats;
    private PickupManager pickupManager;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = GetComponent<PlayerStats>();

        pickupManager = FindFirstObjectByType<PickupManager>();
        pickupManager.OnAllPickupsCollected += OnWin;
    }

    void OnDestroy()
    {
        if (pickupManager != null)
            pickupManager.OnAllPickupsCollected -= OnWin;
    }

    void OnWin()
    {
        GameManager.Instance.WinGame();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            GameManager.Instance.LoseGame(gameObject);

        IceSurface ice = collision.gameObject.GetComponent<IceSurface>();

        if (ice != null)
            movement.SetIce(ice);
    }

    private void OnCollisionExit(Collision collision)
    {
        IceSurface ice = collision.gameObject.GetComponent<IceSurface>();

        if (ice != null)
            movement.SetIce(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        var pickup = other.GetComponent<PickupItem>();

        if (pickup != null)
        {
            pickup.Collect(pickupManager);
            stats.AddScore(pickup.scoreValue);
        }
    }
}