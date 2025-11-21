using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int _enemyCount = 10;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _leftSpawn;
    [SerializeField] private GameObject _rightSpawn;
    [SerializeField] private float _spawnCooldownMin;
    [SerializeField] private float _spawnCooldownMax;
    [SerializeField] private int _damage = 5;

    private EnemySide _leftEnemies;
    private EnemySide _rightEnemies;

    public int Damage => _damage;

    private void Awake()
    {
        ServiceProvider.SetService(this);
    }

    private void Start()
    {
        if (_enemyCount == 0)
        {
            Debug.LogWarning("Enemy count is set to 0!");
            return;
        }

        int leftEnemiesCount = _enemyCount / 2;

        int rightEnemiesCount = _enemyCount - _enemyCount / 2;

        _leftEnemies = _leftSpawn.AddComponent<EnemySide>();
        _rightEnemies = _rightSpawn.AddComponent<EnemySide>();


        _leftEnemies.Init(leftEnemiesCount, Vector2.right, _leftSpawn.transform.position, transform,
                          _enemyPrefab, _spawnCooldownMin, _spawnCooldownMax);

        _rightEnemies.Init(rightEnemiesCount, Vector2.left, _rightSpawn.transform.position, transform,
                           _enemyPrefab, _spawnCooldownMin, _spawnCooldownMax);

        _leftEnemies.Activate();
        _rightEnemies.Activate();
    }
}
