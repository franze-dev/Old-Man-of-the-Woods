using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 150f;
    [SerializeField] private float _counterMovementForce = 2f;
    [SerializeField] private CharacterAnimation _enemyAnimation;
    private Vector2 _dir;
    private Vector2 _counterMovement;
    private Rigidbody2D rb;
    private float _currentSpeed;
    private BoxCollider2D _boxCollider;
    private Enemy _enemy;

    public Vector2 Dir { get => _dir; set => _dir = value; }

    private void StartWalkAnimation()
    {
        _dir.y = 0;

        _enemyAnimation.FlipSprite(-_dir.x);

        _enemyAnimation.SetWalkValue(Mathf.Clamp01(Mathf.Abs(_dir.x)));
    }

    private void Awake()
    {
        _dir = Vector2.zero;
        rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        if (_enemyAnimation == null)
            _enemyAnimation = GetComponent<CharacterAnimation>();

        _enemy = GetComponent<Enemy>();

    }

    private void OnEnable()
    {
        _currentSpeed = _speed;
        StartWalkAnimation();
    }

    private void OnDisable()
    {
        _currentSpeed = 0f;
    }

    private void FixedUpdate()
    {
        _counterMovement.x = -rb.linearVelocity.x * _counterMovementForce;

        if (IsGrounded())
        {
            Debug.Log(_dir.normalized);
            rb.AddForce(_currentSpeed * Time.fixedDeltaTime * _dir.normalized + _counterMovement, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        var floor = new Vector2(_boxCollider.bounds.center.x, _boxCollider.bounds.min.y - 0.01f);

        RaycastHit2D hit = Physics2D.Raycast(floor, Vector2.down, 0.1f);
        return hit.collider != null;
    }
}
