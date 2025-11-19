using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneContainer", menuName = "ScriptableObjects/SceneContainer")]
public class SceneContainer : ScriptableObject
{
    [SerializeField] private int _menuSceneIndex;
    [SerializeField] private int _level1SceneIndex;
    [SerializeField] private int _loadingSceneIndex;
    [SerializeField] private int _bootSceneIndex;

    public int MenuSceneIndex => _menuSceneIndex;
    public int GameplaySceneIndex => _level1SceneIndex;
    public int LoadingSceneIndex => _loadingSceneIndex;
    public int BootSceneIndex => _bootSceneIndex;

#if UNITY_EDITOR
    [SerializeField] private SceneAsset _menuScene;
    [SerializeField] private SceneAsset _gameplayScene;
    [SerializeField] private SceneAsset _loadingScene;
    [SerializeField] private SceneAsset _bootScene;

    /// <summary>
    /// Saves the indexes of the provided scenes
    /// </summary>
    private void OnValidate()
    {
        _menuSceneIndex = SceneLoader.GetSceneIndex(_menuScene);
        _level1SceneIndex = SceneLoader.GetSceneIndex(_gameplayScene);
        _loadingSceneIndex = SceneLoader.GetSceneIndex(_loadingScene);
        _bootSceneIndex = SceneLoader.GetSceneIndex(_bootScene);
    }
#endif
}
