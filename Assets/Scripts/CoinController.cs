using UnityEngine;
using UnityEngine.Serialization;

public class CoinController : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickupSFX;
    [SerializeField] private int _scoreValue = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(_coinPickupSFX, transform.position);
            gameObject.SetActive(false);
            GameSession.Instance.AddScore(_scoreValue);
            Destroy(gameObject);
        }
    }
}
