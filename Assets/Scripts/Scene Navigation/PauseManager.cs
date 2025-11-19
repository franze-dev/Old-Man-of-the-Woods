using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference _pauseInput;

    public static bool Paused { get; set; }

    private void Start()
    {
        Time.timeScale = 1f;

        _pauseInput.action.started += OnPause;

        EventProvider.Subscribe<IPauseEvent>(Pause);
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());
    }

    private void OnDestroy()
    {
        EventProvider.Subscribe<IPauseEvent>(Pause);
    }

    private void Pause(IPauseEvent pauseEvent)
    {
        ServiceProvider.TryGetService<MenuManager>(out var gestion);

        if (gestion.CurrentState != null && gestion.CurrentState is not PauseEvent)
            return;

        Paused = !Paused;

        Time.timeScale = Paused ? 0f : 1f;

        if (Paused)
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new PauseState(), false));
        else
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameEvent(false));
    }
}

public class PauseEvent : IPauseEvent
{
}

public interface IPauseEvent : IEvent
{

}