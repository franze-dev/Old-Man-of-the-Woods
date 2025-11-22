using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegenEffect", menuName = "ScriptableObjects/Shop/HealthRegenEffect")]
public class HealthRegenEffect : ImprovementEffect
{
    [SerializeField] private float _hpPerSecond = 1f;
    public override void Apply(Player player)
    {
        player.AddHealthRegen(_hpPerSecond);
    }
}
