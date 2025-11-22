using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelPopUp : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration = 1f;
    [SerializeField] private float _holdDuration = 0.8f;
    [SerializeField] private float _fadeOutDuration = 1f;
    [SerializeField] private TextMeshProUGUI _levelTextFg;
    [SerializeField] private TextMeshProUGUI _levelTextBg;
    [SerializeField] private float _bgScaleMultiplier = 3f;
    private Vector3 _startBgScale;
    private Coroutine _popupRoutine;
    private LevelManager _levelManager;

    private void Awake()
    {
        _startBgScale = _levelTextBg.transform.localScale;
        _levelTextBg.alpha = 0f;
        _levelTextFg.alpha = 0f;
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out _levelManager);
        EventProvider.Subscribe<ILevelUpEvent>(OnLevelUp);
        EventProvider.Subscribe<IActivateGameplayEvent>(OnActivateGameplay);
        EventProvider.Subscribe<IPauseEvent>(OnPause);
    }

    private void OnActivateGameplay(IActivateGameplayEvent @event)
    {
        ShowLevel(_levelManager.CurrentLevel);
    }

    private void OnPause(IPauseEvent @event)
    {
        if (PauseManager.Paused)
            return;

        if (_popupRoutine != null)
        {
            StopCoroutine(_popupRoutine);
            _popupRoutine = null;
        }

        _levelTextBg.alpha = 0f;
        _levelTextFg.alpha = 0f;
        _levelTextBg.transform.localScale = _startBgScale;
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<ILevelUpEvent>(OnLevelUp);
        EventProvider.Unsubscribe<IPauseEvent>(OnPause);
        EventProvider.Unsubscribe<IActivateGameplayEvent>(OnActivateGameplay);
    }

    private void OnLevelUp(ILevelUpEvent @event)
    {
        if (@event.NewLevel == 1)
            ShowLevel(@event.NewLevel);
    }

    private void ShowLevel(int level)
    {
        SetLevel(level);

        if (_popupRoutine != null)
        {
            StopCoroutine(_popupRoutine);
            _popupRoutine = null;
        }

        _popupRoutine = StartCoroutine(LevelUpPopUpRoutine());
    }

    private IEnumerator LevelUpPopUpRoutine()
    {
        var bgTransform = _levelTextBg.transform;

        Vector3 originalScale = bgTransform.localScale;
        Vector3 targetScale = originalScale * _bgScaleMultiplier;

        _levelTextBg.alpha = 0f;
        _levelTextFg.alpha = 0f;

        bgTransform.localScale = originalScale;

        float currentTime = 0f;

        while (currentTime < _fadeInDuration)
        {
            currentTime += Time.deltaTime;
            var normalized = currentTime / _fadeInDuration;
            _levelTextBg.alpha = normalized;
            _levelTextFg.alpha = normalized;
            bgTransform.localScale = Vector3.Lerp(originalScale, targetScale, normalized);

            yield return null;
        }

        yield return new WaitForSeconds(_holdDuration);

        currentTime = 0f;

        while (currentTime < _fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            float normalized = currentTime / _fadeOutDuration;
            _levelTextBg.alpha = 1f - normalized;
            _levelTextFg.alpha = 1f - normalized;

            yield return null;
        }

        _levelTextBg.alpha = 0f;
        _levelTextFg.alpha = 0f;
        bgTransform.localScale = originalScale;
    }

    public void SetLevel(int level)
    {
        string levelString = "LEVEL " + level;
        _levelTextFg.text = levelString;
        _levelTextBg.text = levelString;
    }
}

public interface ILevelUpEvent : IEvent
{
    public int NewLevel { get; }
}

public class LevelUpEvent : ILevelUpEvent
{
    public int NewLevel { get; private set; }
    public LevelUpEvent(int newLevel)
    {
        NewLevel = newLevel;
    }
}