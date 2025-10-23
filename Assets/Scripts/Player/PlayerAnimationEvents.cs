using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerAttack _playerActions;

    private void Awake()
    {
        if (_playerActions == null)
            _playerActions = GetComponentInParent<PlayerAttack>();
    }

    public void Attack()
    {
        _playerActions.Attack();
    }

}
