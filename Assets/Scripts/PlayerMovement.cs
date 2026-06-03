using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 5f;
    
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private bool isMovingHorizontally;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        Run();
        SetSpriteHorizontalDirection();
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_collider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        
        if (value.isPressed)
        {
            _rigidbody.linearVelocity += new Vector2(0f, jumpPower);
        }
    }

    private void Run()
    {
        _moveVelocity = new Vector2(_moveInput.x * moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = _moveVelocity;
        
        isMovingHorizontally = Math.Abs(_rigidbody.linearVelocity.x) > Mathf.Epsilon;
        
        _animator.SetBool("isRunning", isMovingHorizontally);
    }

    private void SetSpriteHorizontalDirection()
    {
        if (isMovingHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(_moveVelocity.x), 1f);
        }
    }
}
