using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Rigidbody2D _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        _moveVelocity = new Vector2(_moveInput.x * moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = _moveVelocity;
    }
}
