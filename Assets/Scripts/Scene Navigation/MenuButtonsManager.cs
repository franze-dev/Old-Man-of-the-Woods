using UnityEngine;

public class MenuButtonsManager : MonoBehaviour
{
    private MenuManager manager;

    private void Awake()
    {
        ServiceProvider.SetService(this);
    }

    private void Start()
    {
        ServiceProvider.TryGetService(out manager);
    }

    public void ToCredits()
    {
        manager.GoToMenu(new CreditsState());
    }

    public void ToLogs()
    {
        manager.GoToMenu(new LogsState());
    }

    public void ToExit()
    {
        manager.GoToMenu(new ExitState());
    }

    public void ToMainMenu()
    {
        manager.GoToMenu(new MainMenuState());
    }

    public void ToPreviousMenu()
    {
        IMenuState previous = manager.PreviousState;
        manager.GoToMenu(previous);
    }

    public void Pause()
    {
        EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());
    }

    public void ToGame()
    {
        if (manager.CurrentState is PauseState)
            EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());
        EventTriggerer.Trigger<IActivateGameplayEvent>(new ActivateGameplayEvent());
        EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateGameSceneEvent(false));
    }

    public void ExitGame()
    {
        EventTriggerer.Trigger<IExitGameEvent>(new ExitGameEvent());
    }
}