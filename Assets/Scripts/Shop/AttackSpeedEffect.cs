using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpeedEffect", menuName = "ScriptableObjects/Shop/AttackSpeedEffect")]
public class AttackSpeedEffect : ImprovementEffect
{
    [SerializeField] float _speedMultiplier = 1.2f;

    public override void Apply(Player player)
    {
        var attack = player.GetComponent<PlayerAttack>();
        if (attack != null)
        {
            attack.MultiplyAttackSpeed(_speedMultiplier);
        }
    }
}
