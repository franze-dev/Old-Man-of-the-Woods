using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuObject;
    public GameObject PauseMenuObject;
    public GameObject CreditsMenuObject;
    public GameObject ExitMenuObject;
    public GameObject LogsMenuObject;
    public GameObject BackgroundObject;
    public GameObject LoseMenuObject;
    public GameObject WinMenuObject;
    public GameObject ShopMenuObject;
    public GameObject TutorialMenuObject;

    public IMenuState CurrentState = null;
    public IMenuState PreviousState = null;

    private void Awake()
    {
        ServiceProvider.SetService(this, true);
        EventProvider.Subscribe<IPlayerDeathEvent>(OnPlayerDeath);
        EventProvider.Subscribe<IWinGameEvent>(OnWinGame);
        EventProvider.Subscribe<ILevelUpEvent>(OnLevelUp);
    }

    private void OnLevelUp(ILevelUpEvent @event)
    {
        if (@event.NewLevel > 1)
            GoToMenu(new ShopState(@event.NewLevel));
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IPlayerDeathEvent>(OnPlayerDeath);
        EventProvider.Unsubscribe<IWinGameEvent>(OnWinGame);
        EventProvider.Unsubscribe<ILevelUpEvent>(OnLevelUp);
    }

    private void OnWinGame(IWinGameEvent @event)
    {
        GoToMenu(new WinState());
    }

    private void OnPlayerDeath(IPlayerDeathEvent @event)
    {
        GoToMenu(new LoseState());
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

        if (PreviousState == null && CurrentState is not MainMenuState)
            PreviousState = new MainMenuState();

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
        LoseMenuObject.SetActive(false);
        WinMenuObject.SetActive(false);
        ShopMenuObject.SetActive(false);
        TutorialMenuObject.SetActive(false);
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

public interface IWinGameEvent : IEvent
{
}

public class WinGameEvent : IWinGameEvent
{

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

public class LoseState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        MenuManager.ResetGame();
        manager.ShowMenuObject(manager.LoseMenuObject);
    }
}

public class WinState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        MenuManager.ResetGame();
        manager.ShowMenuObject(manager.WinMenuObject);
    }
}

public class TutorialState : IMenuState
{
    public void Enter(MenuManager manager)
    {
        manager.ShowMenuObject(manager.TutorialMenuObject);
    }
}

public class ShopState : IMenuState
{
    int _levelToOpen;

    public ShopState(int levelToOpen)
    {
        _levelToOpen = levelToOpen;
    }

    public void Enter(MenuManager manager)
    {
        ServiceProvider.TryGetService(out ShopMenu shopMenu);
        if (shopMenu != null)
            shopMenu.OpenShop(_levelToOpen);
        EventTriggerer.Trigger<IDeactivateGameplayEvent>(new DeactivateGameplayEvent());
        manager.ShowMenuObject(manager.ShopMenuObject);
    }
}

public class DeactivateGameplayEvent : IDeactivateGameplayEvent
{
}

public interface IDeactivateGameplayEvent : IEvent
{
}