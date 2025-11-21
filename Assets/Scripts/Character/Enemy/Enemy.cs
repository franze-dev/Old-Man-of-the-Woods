using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private CharacterAnimation _characterAnimation;

    private void OnEnable()
    {
        _startPos = transform.position;
    }

    private void OnDisable()
    {
        transform.position = _startPos;
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageRoutine(damage));
    }

    private void Die()
    {
        gameObject.SetActive(false);
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.AddCoins();
        _currentHealth = _maxHealth;
    }

    private IEnumerator TakeDamageRoutine(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        _characterAnimation.TriggerHurt();

        yield return new WaitForSeconds(1f);

        if (_currentHealth <= 0)
        {
            _currentHealth = _maxHealth;
            _characterAnimation.TriggerDeath();
            yield return new WaitForSeconds(1f);
            Die();
        }

    }
}