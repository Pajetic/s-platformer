using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSFX;

private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(coinPickupSFX, transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
