using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public int scoreValue = 1;
    public AudioSource collectSound;

    public void Collect(PickupManager manager)
    {
        AudioManager.Instance.PlaySFX(collectSound.clip);

        manager.OnPickupCollected(this);

        Destroy(gameObject);
    }
}