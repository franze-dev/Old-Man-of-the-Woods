using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _finalLevel = 3;
    [SerializeField] private float _levelTimer = 60f;
    [SerializeField] GameObject _gameplayGO;
    [SerializeField] private AssetReferenceGameObject _environmentRef;
    [SerializeField] private Transform _environmentPos;
    private GameObject _environmentInstance;

    private float _timer;
    private bool _timerStarted;
    private bool _gameplayActive = true;
    private ArchievementsManager _archievementsManager;

    public bool TimerStarted => _timerStarted;

    public float Timer => _timer;

    public float MaxTimer => _levelTimer;

    public int CurrentLevel => _currentLevel;

    private void Awake()
    {
        ServiceProvider.SetService(this, true);
        EventProvider.Subscribe<IDeactivateGameplayEvent>(OnDeactivateGameplay);
        EventProvider.Subscribe<IActivateGameplayEvent>(OnActivateGameplay);
        StartCoroutine(InstanceEnvironment());
        _timerStarted = false;
    }

    private IEnumerator InstanceEnvironment()
    {
        var op = _environmentRef.InstantiateAsync(_environmentPos);

        ServiceProvider.TryGetService<SceneFlowManager>(out var manager);

        if (!manager)
            yield return op;
        else
            yield return manager.UseLoad(op);

        _environmentInstance = op.Result;
    }

    private void OnActivateGameplay(IActivateGameplayEvent @event)
    {
        if (!_gameplayActive)
        {
            _gameplayActive = true;
            _gameplayGO.SetActive(true);
        }
    }

    private void OnDeactivateGameplay(IDeactivateGameplayEvent @event)
    {
        if (_gameplayActive)
        {
            _gameplayActive = false;
            _gameplayGO.SetActive(false);
        }
    }

    private void Start()
    {
        EventProvider.Subscribe<ILevelUpEvent>(OnLevelUp);
        EventTriggerer.Trigger<ILevelUpEvent>(new LevelUpEvent(_currentLevel));

        ServiceProvider.TryGetService(out _archievementsManager);

        _timer = _levelTimer;
    }

    private void OnDestroy()
    {
        _environmentRef.ReleaseInstance(_environmentInstance);

        EventProvider.Unsubscribe<ILevelUpEvent>(OnLevelUp);
        EventProvider.Unsubscribe<IActivateGameplayEvent>(OnActivateGameplay);
        EventProvider.Unsubscribe<IDeactivateGameplayEvent>(OnDeactivateGameplay);
    }
    public void StartLevelTimer()
    {
        _timerStarted = true;
    }

    private void Update()
    {
        if (!_timerStarted)
            return;

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _timer = _levelTimer;
            EventTriggerer.Trigger<ILevelUpEvent>(new LevelUpEvent(_currentLevel + 1));
        }
    }

    private void OnLevelUp(ILevelUpEvent @event)
    {
        if (_currentLevel == @event.NewLevel)
            return;

        _timerStarted = false;

        _timer = _levelTimer;

        _currentLevel = @event.NewLevel;

        _archievementsManager.ArchieveLevel(_currentLevel);

        if (_currentLevel > _finalLevel)
        {
            _currentLevel = 0;
            EventTriggerer.Trigger<IWinGameEvent>(new WinGameEvent());
            return;
        }
    }
}

public interface IActivateGameplayEvent : IEvent
{
}

public class ActivateGameplayEvent : IActivateGameplayEvent
{

}