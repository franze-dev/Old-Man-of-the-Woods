using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthEffect", menuName = "ScriptableObjects/Shop/MaxHealthEffect")]
public class MaxHealthEffect : ImprovementEffect
{
    [SerializeField] private int _addAmount = 25;

    public override void Apply(Player player)
    {
        player.AddMaxHealth(_addAmount);
    }
}
