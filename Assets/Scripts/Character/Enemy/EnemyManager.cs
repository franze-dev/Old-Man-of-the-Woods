using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int _enemyCount = 10;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _leftSpawn;
    [SerializeField] private GameObject _rightSpawn;
    [SerializeField] private float _spawnCooldownMin;
    [SerializeField] private float _spawnCooldownMax;

    private EnemySide _leftEnemies;
    private EnemySide _rightEnemies;

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

public class EnemySide : MonoBehaviour
{
    private List<Enemy> _enemies;
    private Vector2 _dir;
    private float _spawnCooldownMin;
    private float _spawnCooldownMax;
    private bool _enabled;
    private Vector3 _startPos;
    private int _startCount;
    private Transform _parent;
    private GameObject _enemyPrefab;

    public void Init(int count, Vector2 dir, Vector3 pos, Transform parent, GameObject enemyPrefab,
                     float cooldownMin, float cooldownMax)
    {
        _enemies = new List<Enemy>();
        _spawnCooldownMin = cooldownMin;
        _spawnCooldownMax = cooldownMax;
        _startPos = pos;
        _startCount = count;
        _parent = parent;
        _enemyPrefab = enemyPrefab;
        _dir = dir;
        _enabled = true;

        AddEnemies(count);
    }

    public void Activate()
    {
        StartCoroutine(SpawnEnemiesCoroutine());
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (_enemies.Count > 0 && _enabled)
        {
            var secs = UnityEngine.Random.Range(_spawnCooldownMin, _spawnCooldownMax);
            yield return new WaitForSeconds(secs);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null && !enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(true);
                return;
            }
        }
    }

    private void Reset()
    {
        foreach (var enemy in _enemies)
        {
            enemy.transform.position = _startPos;
            enemy.gameObject.SetActive(false);
        }

        if (_enemies.Count < _startCount)
            AddEnemies(_startCount - _enemies.Count);
    }

    private void AddEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var enemyObj = GameObject.Instantiate(_enemyPrefab, _startPos, Quaternion.identity, _parent);
            var enemyMovement = enemyObj.GetComponent<EnemyMovement>();
            enemyMovement.Dir = _dir;
            enemyObj.SetActive(false);
            _enemies.Add(enemyObj.GetComponent<Enemy>());
        }
    }
}
