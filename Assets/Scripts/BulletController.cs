using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 20f;
    private Vector2 _projectileVector;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidbody.linearVelocity = _projectileVector;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject, 0.2f);
    }

    public void SetProjectileDirection(float direction)
    {
        _projectileVector = new Vector2(direction * projectileSpeed, 0f);
    }
}
