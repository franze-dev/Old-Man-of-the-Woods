using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _isFacingRight = false;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
            if (!_animator.enabled)
                _animator.enabled = true;
        }
    }

    public void SetWalkValue(float value)
    {
        _animator.SetFloat("xVelocity", value);
    }

    public void FlipSprite(float dir)
    {
        if (_isFacingRight && dir > 0f || !_isFacingRight && dir < 0f)
        {
            _isFacingRight = !_isFacingRight;

            Vector2 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    public void TriggerAttack()
    {
        _animator.SetTrigger("attack");
    }

    public void TriggerHurt()
    {
        _animator.SetTrigger("hurt");
    }
}
