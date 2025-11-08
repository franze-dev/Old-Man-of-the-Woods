using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneLoader : MonoBehaviour
{
    private List<Scene> sceneList;

    private void Awake()
    {
        EventProvider.Subscribe<IExitGameEvent>(ExitGame);

        sceneList = new()
        {
            SceneManager.GetActiveScene()
        };
    }

    private void OnDestroy()
    {
        UnloadAll();
        EventProvider.Unsubscribe<IExitGameEvent>(ExitGame);
    }

    public static void ExitGame(IExitGameEvent @event)
    {
        Application.Quit();
    }

    public void LoadScene(int newScene, int transitionScene, bool unloadPrevious = true)
    {
        StartCoroutine(LoadSceneCorroutine(newScene, transitionScene, unloadPrevious));
    }

    public int ActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        return (int)activeScene.buildIndex;
    }

    public void UnloadAll()
    {
        foreach (var scene in sceneList)
            if (scene.IsValid() && scene.isLoaded)
                SceneManager.UnloadSceneAsync(scene);

        sceneList.Clear();
    }

    public IEnumerator LoadSceneCorroutine(int newScene, int transition, bool unloadPrevious = true)
    {
        if (IsLoaded(newScene))
            yield break;

        var anterior = ActiveScene();

        if (unloadPrevious)
        {
            yield return SceneManager.UnloadSceneAsync(anterior);
            yield return null;
        }

        var loading = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        if (!IsLoaded(transition))
        {
            yield return SceneManager.LoadSceneAsync(transition, LoadSceneMode.Additive);
            sceneList.Add(GetScene(transition));
        }

        ActivateScene(transition);

        while (!loading.isDone)
        {
            EventTriggerer.Trigger<ILoadEvent>(new LoadEvent(loading.progress));
            yield return null;
        }

        yield return loading;
        sceneList.Add(GetScene(newScene));
        yield return null;

        ActivateScene(newScene);
        EventTriggerer.Trigger<IResetLoadEvent>(new ResetLoadEvent());
    }

    public IEnumerator UseLoad(AsyncOperationHandle<GameObject> op, int transition, int nextToActivate)
    {
        var loading = op;

        if (!IsLoaded(transition))
        {
            yield return SceneManager.LoadSceneAsync(transition, LoadSceneMode.Additive);
            sceneList.Add(GetScene(transition));
        }

        ActivateScene(transition);

        while (!loading.IsDone)
        {
            EventTriggerer.Trigger<ILoadEvent>(new LoadEvent(loading.PercentComplete));
            yield return null;
        }

        yield return loading;

        ActivateScene(nextToActivate);
        EventTriggerer.Trigger<IResetLoadEvent>(new ResetLoadEvent());
    }

    private Scene GetScene(int index)
    {
        return SceneManager.GetSceneByBuildIndex(index);
    }

    public Scene GetSceneInList(int newScene)
    {
        Scene scene = new();

        foreach (var myScene in sceneList)
        {
            scene = myScene;
            bool isTheSame = myScene.buildIndex == (int)newScene;
            if (isTheSame)
                return myScene;
        }

        Debug.LogWarning($"{newScene} is not loaded yet");
        return scene;
    }


    public bool ActivateScene(int newScene)
    {
        Scene scene = GetSceneInList(newScene);

        if (scene.buildIndex != (int)newScene)
            return false;

        SceneManager.SetActiveScene(scene);
        return true;
    }

    private void UnloadScene(Scene scene)
    {
        sceneList.Remove(scene);

        if (!scene.isLoaded)
        {
            Debug.LogWarning($"Tried to unload a scene that is already unloaded. Did not do it. Scene state: {scene}");
            return;
        }

        SceneManager.UnloadSceneAsync(scene);
    }

    public void UnloadScene(int newScene)
    {
        Scene scene = GetSceneInList(newScene);

        if (scene.buildIndex != (int)newScene)
            return;

        UnloadScene(scene);
    }


    public bool IsLoaded(int index)
    {
        foreach (var sceme in sceneList)
        {
            bool isTheSame = sceme.buildIndex == (int)index;
            if (isTheSame)
                return true;
        }
        return false;
    }

    public bool IsGameplay(int index)
    {
        return index == GameplaySceneData.Index;
    }

#if UNITY_EDITOR    
    public static int GetSceneIndex(SceneAsset asset)
    {
        if (!asset)
            return 0;

        return SceneUtility.GetBuildIndexByScenePath(AssetDatabase.GetAssetPath(asset));
    }
#endif
}
