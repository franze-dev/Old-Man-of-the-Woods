using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    [SerializeField] private float _invincibilityDuration = 1.0f;
    [SerializeField] private CharacterAnimation _characterAnimation;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _invincibilityColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private float _blinkInterval = 0.1f;
    [SerializeField] private Image _healthFill;
    private float _invincibilityTimer = 0.0f;
    private Coroutine _invincibilityCoroutine;
    private EnemyManager _enemyManager;

    private void Start()
    {
        if (_characterAnimation == null)
            _characterAnimation = GetComponent<CharacterAnimation>();

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (_healthFill == null)
            Debug.LogError("Health Fill Image is not assigned in the Player script.");

        ServiceProvider.TryGetService<EnemyManager>(out _enemyManager);

        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        if (_invincibilityTimer <= 0)
            StartCoroutine(TakeDamageRoutine(damage));
    }

    private IEnumerator TakeDamageRoutine(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        UpdateHealthBar();
        _characterAnimation?.TriggerHurt();
        yield return new WaitForSeconds(1f);

        if (_currentHealth <= 0)
        {
            _characterAnimation.TriggerDeath();
            yield return new WaitForSeconds(1f);
            Die();
            yield break;
        }

        _invincibilityTimer = _invincibilityDuration;

        DoInvicibilityBlink();
    }

    private void UpdateHealthBar()
    {
        if (_healthFill != null)
        {
            float fillAmount = (float)_currentHealth / _maxHealth;
            _healthFill.fillAmount = fillAmount;
        }
    }

    private void DoInvicibilityBlink()
    {
        if (_invincibilityCoroutine != null)
        {
            StopCoroutine(_invincibilityCoroutine);
            _invincibilityCoroutine = null;
        }

        _invincibilityCoroutine = StartCoroutine(InvincibilityBlinkRoutine());
    }

    private IEnumerator InvincibilityBlinkRoutine()
    {
        while (_invincibilityTimer > 0f)
        {
            _spriteRenderer.color = _invincibilityColor;
            yield return new WaitForSeconds(_blinkInterval);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(_blinkInterval);
        }

        _spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        if (_invincibilityTimer > 0f)
            _invincibilityTimer -= Time.deltaTime;
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        UpdateHealthBar();
    }

    private void Die()
    {
        EventTriggerer.Trigger<IPlayerDeathEvent>(new PlayerDeathEvent());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyDamage"))
            TakeDamage(_enemyManager.Damage);
    }
}

internal class PlayerDeathEvent : IPlayerDeathEvent
{
}

internal interface IPlayerDeathEvent : IEvent
{
}