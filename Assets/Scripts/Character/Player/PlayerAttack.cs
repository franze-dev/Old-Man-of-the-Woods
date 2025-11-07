using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputActionReference _attackAction;
    [SerializeField] private CharacterAnimation _playerAnimation;
    [SerializeField] private float _currentTime = 0f;
    [SerializeField] private float _startAttackValue = 0.3f;
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _attackRadius = 1f;
    [SerializeField] private LayerMask _whatIsEnemies;
    [SerializeField] private float _damage = 25f;

    private List<Collider2D> _toDamage;

    private void Awake()
    {
        if (_playerAnimation == null)
            _playerAnimation = GetComponent<CharacterAnimation>();

        _attackAction.action.started += OnAttack;
    }

    private void OnDisable()
    {
        _attackAction.action.started -= OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        bool isPlaying = _playerAnimation.IsPlaying("atk");

        if (_currentTime <= 0 && !isPlaying)
        {
            //_toDamage = Physics2D.OverlapCircleAll(_attackOrigin.position, _attackRadius, _whatIsEnemies).ToList();
            _currentTime = _startAttackValue;
            _playerAnimation.TriggerAttack();
        }
    }

    public void Attack()
    {
        _toDamage = Physics2D.OverlapCircleAll(_attackOrigin.position, _attackRadius, _whatIsEnemies).ToList();

        foreach (var enemy in _toDamage)
        {
            var enemyScript = enemy.GetComponent<Enemy>();

            enemyScript?.TakeDamage(_damage);
        }
    }

    private void Update()
    {
        if (_currentTime > -_startAttackValue)
            _currentTime -= Time.deltaTime;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_attackOrigin != null)
            Gizmos.DrawWireSphere(_attackOrigin.position, _attackRadius);
    }
}
