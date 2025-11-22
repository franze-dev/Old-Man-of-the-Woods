using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private static bool _levelStarted = false;
    private LevelManager _levelManager;

    private void Start()
    {
        ServiceProvider.TryGetService(out _levelManager);
        EventProvider.Subscribe<IActivateGameplayEvent>(OnActivateGameplay);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IActivateGameplayEvent>(OnActivateGameplay);
    }

    private void OnActivateGameplay(IActivateGameplayEvent @event)
    {
        Activate();
    }

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

                if (!_levelManager.TimerStarted)
                    _levelManager.StartLevelTimer();

                return;
            }
        }
    }

    public void Reset()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy == null)
                continue;

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
