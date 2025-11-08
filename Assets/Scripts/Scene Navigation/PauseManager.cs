using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private KeyCode TeclaPausa = KeyCode.Escape;

    public static bool Paused { get; set; }

    private void Start()
    {
        Time.timeScale = 1f;

        EventProvider.Subscribe<IPauseEvent>(Pausar);
    }

    private void Update()
    {
        if (Input.GetKeyUp(TeclaPausa))
            EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());
    }
    private void OnDestroy()
    {
        EventProvider.Subscribe<IPauseEvent>(Pausar);
    }

    private void Pausar(IPauseEvent pauseEvent)
    {
        ServiceProvider.TryGetService<MenuManager>(out var gestion);

        if (gestion.CurrentState != null && gestion.CurrentState is not PauseEvent)
            return;

        Paused = !Paused;

        Time.timeScale = Paused ? 0f : 1f;

        if (Paused)
            EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new PauseMenu(), false));
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