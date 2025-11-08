using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(SceneLoader))]
public class SceneFlowManager : MonoBehaviour
{
    [SerializeField] public SceneContainer Container;
    private SceneLoader Loader;

    private void Awake()
    {
        ServiceProvider.SetService(this);

        Loader = GetComponent<SceneLoader>();

        EventProvider.Subscribe<IActivateSceneEvent>(ActivateScene);
    }

    private void OnDestroy()
    {
        EventProvider.Unsubscribe<IActivateSceneEvent>(ActivateScene);
    }

    public void ActivateScene(IActivateSceneEvent @event)
    {
        int index = @event.Index;

        var activeSceneIndex = Loader.ActiveScene();

        if (activeSceneIndex == index)
        {
            Debug.Log("Tried to load a scene twice. " + activeSceneIndex);
            return;
        }

        if (@event.Index == GameplaySceneData.Index)
        {
            ServiceProvider.TryGetService<MenuManager>(out var manager);
            manager.CurrentState = null;
        }

        if (!IsLoaded(index))
            Loader.LoadScene(index, Container.LoadingSceneIndex, @event.UnloadPrevious);
        else
            Loader.ActivateScene(index);
    }

    public IEnumerator UsarCarga(AsyncOperationHandle<GameObject> op)
    {
        return Loader.UseLoad(op, Container.LoadingSceneIndex, Container.EscenaJuegoIndice);
    }

    public bool IsLoaded(int index)
    {
        return Loader.IsLoaded(index);
    }

    public bool EsJuego(int index)
    {
        return Loader.IsGameplay(index);
    }

    public void DescargarEscena(int index)
    {
        Loader.UnloadScene(index);
    }
}

public interface IExitGameEvent : IEvent
{
}

/// <summary>
/// Event that is called to exit the game
/// </summary>
public class ExitGameEvent : IExitGameEvent
{
}