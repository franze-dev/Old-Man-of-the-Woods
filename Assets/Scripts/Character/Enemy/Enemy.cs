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
        _characterAnimation.TriggerDeath();
    }

    private IEnumerator TakeDamageRoutine(float damage)
    {
        _currentHealth -= damage;
        _characterAnimation.TriggerHurt();

        yield return new WaitForSeconds(1f);

        if (_currentHealth <= 0)
        {
            _currentHealth = _maxHealth;
            Die();
        }

        gameObject.SetActive(false);
    }
}