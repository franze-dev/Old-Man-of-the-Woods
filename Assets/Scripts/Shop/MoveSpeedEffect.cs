using UnityEngine;

[CreateAssetMenu(fileName = "MoveSpeedEffect", menuName = "ScriptableObjects/Shop/MoveSpeedEffect")]
public class MoveSpeedEffect : ImprovementEffect
{
    [SerializeField] private float _speedMultiplier = 1.25f;
    public override void Apply(Player player)
    {
        var movement = player.GetComponent<PlayerMovement>();

        if (movement != null)
            movement.MultiplyMoveSpeed(_speedMultiplier);
    }
}
