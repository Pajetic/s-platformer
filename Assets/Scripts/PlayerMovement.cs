using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private Vector2 deathVector = new Vector2(10f, 20f);
    
    private Vector2 _moveInput;
    private Vector2 _moveVelocity;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _feetCollider;
    private bool _isMovingHorizontally;
    private float _normalGravity;
    private bool _isAlive = true;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();
        _normalGravity = _rigidbody.gravityScale;
    }

    private void Update()
    {
        if (!_isAlive) return;
        Run();
        SetSpriteHorizontalDirection();
        ClimbLadder();
        HandleDeath();
    }

    private void OnMove(InputValue value)
    {
        if (!_isAlive) return;
        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        
        if (value.isPressed)
        {
            _rigidbody.linearVelocity += new Vector2(0f, jumpPower);
        }
    }

    private void Run()
    {
        _moveVelocity = new Vector2(_moveInput.x * moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = _moveVelocity;
        
        _isMovingHorizontally = Math.Abs(_rigidbody.linearVelocity.x) > Mathf.Epsilon;
        
        _animator.SetBool("isRunning", _isMovingHorizontally);
    }

    private void SetSpriteHorizontalDirection()
    {
        if (_isMovingHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(_moveVelocity.x), 1f);
        }
    }

    private void ClimbLadder()
    {
        if (!_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            _rigidbody.gravityScale = _normalGravity;
            _animator.SetBool("isClimbing", false);
            return;
        }

        _rigidbody.gravityScale = 0f;
        _moveVelocity = new Vector2(_rigidbody.linearVelocity.x, _moveInput.y * climbSpeed);
        _rigidbody.linearVelocity = _moveVelocity;
        
        _animator.SetBool("isClimbing", Mathf.Abs(_rigidbody.linearVelocity.y) > Mathf.Epsilon);
    }

    private void HandleDeath()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            _isAlive = false;
            _animator.SetTrigger("isDying");
            _rigidbody.linearVelocity = deathVector;
        }
    }
}
