using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _climbSpeed = 5f;
    [SerializeField] private Vector2 _deathVector = new Vector2(10f, 20f);
    [SerializeField] private Transform _muzzle;
    [SerializeField] private GameObject _bulletPrefab;
    
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
        if (!_isAlive || !_feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        
        if (value.isPressed)
        {
            _rigidbody.linearVelocity += new Vector2(0f, _jumpPower);
        }
    }

    private void OnAttack(InputValue value)
    {
        if (!_isAlive) return;
        Instantiate(_bulletPrefab, _muzzle.position, transform.rotation).GetComponent<BulletController>().SetProjectileDirection(transform.localScale.x > 0 ? 1f : -1f);
    }

    private void Run()
    {
        _moveVelocity = new Vector2(_moveInput.x * _moveSpeed, _rigidbody.linearVelocity.y);
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
        _moveVelocity = new Vector2(_rigidbody.linearVelocity.x, _moveInput.y * _climbSpeed);
        _rigidbody.linearVelocity = _moveVelocity;
        
        _animator.SetBool("isClimbing", Mathf.Abs(_rigidbody.linearVelocity.y) > Mathf.Epsilon);
    }

    private void HandleDeath()
    {
        if (_bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            _isAlive = false;
            _animator.SetTrigger("isDying");
            _rigidbody.linearVelocity = _deathVector;
            GameSession.Instance.HandlePlayerDeath();
        }
    }
}
