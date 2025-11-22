using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _counterMovementForce = 2f;
    [SerializeField] private CharacterAnimation _playerAnimation;

    private Vector2 _dir;
    private Vector2 _counterMovement;
    private BoxCollider2D _boxCollider;
    private float _moveSpeedMultiplier = 1f;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        if (_playerAnimation == null)
            _playerAnimation = GetComponent<CharacterAnimation>();

        _dir = Vector2.zero;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        _dir = Vector2.zero;
        _moveAction.action.started += OnMove;
        _moveAction.action.canceled += OnMove;
    }

    private void OnDisable()
    {
        _dir = Vector2.zero;
        _moveAction.action.started -= OnMove;
        _moveAction.action.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
        _dir.Normalize();
        _dir.y = 0;

        _playerAnimation.FlipSprite(_dir.x);

        Debug.Log($"Move Input: {_dir}");

        _playerAnimation.SetWalkValue(Mathf.Clamp01(Mathf.Abs(_dir.x)));

    }

    private void FixedUpdate()
    {
        if (_dir != Vector2.zero)
        {
            if (!IsGrounded())
            {
                var linearVel = _rb.linearVelocity;
                linearVel.x = 0;
                _rb.linearVelocity = linearVel;
            }

            _counterMovement.x = -_rb.linearVelocity.x * _counterMovementForce;
            _counterMovement.y = -_rb.linearVelocity.y * _counterMovementForce;

            var speed = (_moveSpeed * _moveSpeedMultiplier) * Time.fixedDeltaTime * _dir + _counterMovement;

            _rb.AddForce(speed, ForceMode2D.Impulse);

        }
        else if (_rb.linearVelocity != Vector2.zero && IsGrounded())
            _rb.linearVelocity = Vector2.zero;
    }

    private bool IsGrounded()
    {
        var floor = new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.min.y - 0.01f);

        RaycastHit2D hit = Physics2D.Raycast(floor, Vector2.down, 0.1f);
        return hit.collider != null;
    }

    public void MultiplyMoveSpeed(float multiplier)
    {
        _moveSpeedMultiplier *= multiplier;
    }
}
