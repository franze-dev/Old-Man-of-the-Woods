using UnityEngine;

public interface IActivateSceneEvent : IEvent
{
    int Index { get; }
    bool UnloadPrevious { get; set; }
}

public class ActivateGameEvent : IActivateSceneEvent
{
    public int Index => GameplaySceneData.Index;
    public bool UnloadPrevious { get; set; }

    public ActivateGameEvent(bool unloadPrevious = true)
    {
        UnloadPrevious = unloadPrevious;

        if (PauseManager.Paused)
            EventTriggerer.Trigger<IPauseEvent>(new PauseEvent());

        Time.timeScale = 1;

        if (!UnloadPrevious)
        {
            ServiceProvider.TryGetService<MenuManager>(out var manager);
            manager.HideMenuObjects();
        }
    }
}

public class ActivateMenuEvent : IActivateSceneEvent
{
    public int Index => MenuSceneData.Index;
    public bool UnloadPrevious { get; set; }
    public IMenuState NextState { get; set; }

    public ActivateMenuEvent(IMenuState next, bool unloadPrevious = true)
    {
        this.UnloadPrevious = unloadPrevious;
        NextState = next;

        ServiceProvider.TryGetService<MenuManager>(out var menu);

        menu?.GoToMenu(next);
    }
}

