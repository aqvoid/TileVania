using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;
    [SerializeField] private int pointAddForPickup = 100;

    private bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;
            FindFirstObjectByType<GameSession>().AddToScore(pointAddForPickup);
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            Destroy(gameObject);
        }
    }

}
