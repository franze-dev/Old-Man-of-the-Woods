using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "ScriptableObjects/Shop/DamageEffect")]
public class DamageEffect : ImprovementEffect
{
    [SerializeField] private float _multiplier = 1.3f;

    public override void Apply(Player player)
    {
        var attack = player.GetComponent<PlayerAttack>();

        if (attack != null)
            attack.MultiplyDamage(_multiplier);
    }
}
