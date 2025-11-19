using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuObject;
    public GameObject PauseMenuObject;
    public GameObject CreditsMenuObject;
    public GameObject ExitMenuObject;
    public GameObject LogsMenuObject;
    public GameObject BackgroundObject;

    public IMenuState CurrentState = null;
    public IMenuState PreviousState = null;

    private void Awake()
    {
        ServiceProvider.SetService(this);
    }

    private void Start()
    {
        HideMenuObjects();

        if (CurrentState == null)
            GoToMenu(new MainMenuState());
    }

    public void GoToMenu(IMenuState state)
    {
        if (CurrentState == state)
            return;

        if (CurrentState != null && state is PauseEvent)
            return;

        PreviousState = CurrentState;
        CurrentState = state;
        HideMenuObjects();
        BackgroundObject.SetActive(true);

        CurrentState.Enter(this);
    }

    public void HideMenuObjects()
    {
        MainMenuObject.SetActive(false);
        PauseMenuObject.SetActive(false);
        CreditsMenuObject.SetActive(false);
        ExitMenuObject.SetActive(false);
        LogsMenuObject.SetActive(false);
        BackgroundObject.SetActive(false);
    }

    public void ShowMenuObject(GameObject obj)
    {
        HideMenuObjects();
        BackgroundObject.SetActive(true);
        obj.SetActive(true);
    }

    public static void ResetGame()
    {
        PauseManager.Paused = false;

        ServiceProvider.TryGetService<SceneFlowManager>(out var manager);
        manager.UnloadScene(GameplaySceneData.Index);
    }
}

public interface IMenuState
{
    void Enter(MenuManager manager);
}

public class MainMenuState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        if (PauseManager.Paused)
            EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());

        MenuManager.ResetGame();

        manager.ShowMenuObject(manager.MainMenuObject);
    }
}

public class PauseState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        PauseManager.Paused = true;

        manager.ShowMenuObject(manager.PauseMenuObject);
    }
}

public class CreditsState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        manager.ShowMenuObject(manager.CreditsMenuObject);
    }
}

public class ExitState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        manager.ShowMenuObject(manager.ExitMenuObject);
    }
}

public class LogsState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        manager.ShowMenuObject(manager.LogsMenuObject);
    }
}