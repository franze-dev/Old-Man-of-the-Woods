using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _counterMovementForce = 2f;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private Vector2 _dir;
    private Vector2 _counterMovement;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        if (_playerAnimation == null)
            _playerAnimation = GetComponent<PlayerAnimation>();

        _moveAction.action.started += OnMove;
        _moveAction.action.canceled += OnMove;

        _dir = Vector2.zero;
    }

    private void OnDisable()
    {
        _moveAction.action.started -= OnMove;
        _moveAction.action.canceled -= OnMove;
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        _dir = context.ReadValue<Vector2>();
        _dir.Normalize();
        _dir.y = 0;

        _playerAnimation.FlipSprite(_dir.x);

        _playerAnimation.SetWalkValue(Mathf.Clamp01(Mathf.Abs(_dir.x)));

    }

    private void FixedUpdate()
    {

        if (_dir != Vector2.zero)
        {
            _counterMovement.x = -_rb.linearVelocity.x * _counterMovementForce;
            _counterMovement.y = -_rb.linearVelocity.y * _counterMovementForce;

            var speed = _moveSpeed * Time.fixedDeltaTime * _dir + _counterMovement;

            _rb.AddForce(speed, ForceMode2D.Impulse);

        }
        else if (_rb.linearVelocity != Vector2.zero)
            _rb.linearVelocity = Vector2.zero;
    }
}
