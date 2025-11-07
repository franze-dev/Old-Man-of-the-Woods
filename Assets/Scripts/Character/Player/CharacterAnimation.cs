using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private SpriteRenderer _spriteRenderer;
    private bool _isFacingRight = false;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
            if (!_animator.enabled)
                _animator.enabled = true;
        }

        if (!TryGetComponent(out _spriteRenderer))
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
    private string GetCurrentAnimationName()
    {
        var info = _animator.GetCurrentAnimatorClipInfo(0);
        var clip = info[0].clip;
        Debug.Log(clip.name);
        return clip.name;
    }

    public bool IsPlaying(string animationName)
    {
        var animName = GetCurrentAnimationName();

        return animationName == animName;
    }

    public void TriggerDeath()
    {
        _animator.SetBool("isDead", true);
    }
}
