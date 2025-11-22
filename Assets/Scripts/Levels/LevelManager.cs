using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _finalLevel = 3;
    [SerializeField] private float _levelTimer = 60f;
    [SerializeField] GameObject _gameplayGO;
    private float _timer;
    private bool _timerStarted;
    private bool _gameplayActive = true;

    public bool TimerStarted => _timerStarted;

    public float Timer => _timer;

    public float MaxTimer => _levelTimer;

    public int CurrentLevel => _currentLevel;

    private void Awake()
    {
        ServiceProvider.SetService(this, true);
        EventProvider.Subscribe<IDeactivateGameplayEvent>(OnDeactivateGameplay);
        EventProvider.Subscribe<IActivateGameplayEvent>(OnActivateGameplay);
        _timerStarted = false;
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

        _timer = _levelTimer;
    }

    private void OnDestroy()
    {
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