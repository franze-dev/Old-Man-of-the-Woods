using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerActions _playerActions;

    private void Awake()
    {
        if (_playerActions == null)
            _playerActions = GetComponentInParent<PlayerActions>();
    }

    public void Attack()
    {
        _playerActions.Attack();
    }

}
