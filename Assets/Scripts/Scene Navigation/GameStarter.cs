using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Awake()
    {
        EventProvider.Subscribe<IStartGameEvent>(StartGame);
    }

    private void StartGame(IStartGameEvent juego)
    {
        EventTriggerer.Trigger<IActivateSceneEvent>(new ActivateMenuEvent(new MainMenuState(), false));

        Destroy(gameObject);
    }
}

public class StartGameEvent : IStartGameEvent
{
}

public interface IStartGameEvent : IEvent
{
}