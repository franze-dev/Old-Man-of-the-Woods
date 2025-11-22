using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseInput;
    [SerializeField] private GameObject _uiGO;

    public static bool Paused { get; set; }

    private void Awake()
    {
        EventProvider.Subscribe<IPauseEvent>(Pause);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        _pauseInput.action.started += OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IPauseEvent>(Pause);
    }

    private void Pause(IPauseEvent pauseEvent)
    {
        ServiceProvider.TryGetService<MenuManager>(out var gestion);

        Paused = !Paused;

        Time.timeScale = Paused ? 0f : 1f;

        if (Paused)
        {
            _uiGO.SetActive(false);
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new PauseState(), false));
        }
        else
        {
            _uiGO.SetActive(true);
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameSceneEvent(false));
        }
    }

    public void ReactivateUI()
    {
        _uiGO.SetActive(true);
    }

    public void PauseMenu()
    {
        EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());
    }
}

public class PauseEvent : IPauseEvent
{
}

public interface IPauseEvent : IEvent
{

}