using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        StartGame();
    }

    private void StartGame()
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