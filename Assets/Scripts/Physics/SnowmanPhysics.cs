using UnityEngine;

public class Snowman : MonoBehaviour
{
    public Rigidbody SnowmanRigidbody;

    void Start()
    {

        SnowmanRigidbody.isKinematic = true; //disable physics at the start
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ActivatePhysics();
        }
    }

    void ActivatePhysics()
    {
        SnowmanRigidbody.isKinematic = false; //enable physics when the player collides with the snowman
    }
}