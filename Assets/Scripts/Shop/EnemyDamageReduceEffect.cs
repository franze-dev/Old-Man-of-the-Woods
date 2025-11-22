using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDamageReduceEffect", menuName = "ScriptableObjects/Shop/EnemyDamageReduceEffect")]
public class EnemyDamageReduceEffect : ImprovementEffect
{
    [SerializeField] private float _multiplier = 0.8f;  

    public override void Apply(Player player)
    {
        if (ServiceProvider.TryGetService(out EnemyManager manager))
            manager.MultiplyDamage(_multiplier);
    }
}