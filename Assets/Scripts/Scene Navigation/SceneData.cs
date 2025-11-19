public interface SceneData
{
    /// <summary>
    /// Saves the index of the scene
    /// </summary>
    static int Index { get; }
}

public class MenuSceneData : SceneData
{
    public static int Index => ServiceProvider.TryGetService<SceneFlowManager>(out var controller) ? 
                               controller.Container.MenuSceneIndex : 0;
}

public class GameplaySceneData : SceneData
{
    public static int Index => ServiceProvider.TryGetService<SceneFlowManager>(out var controller) ?
                               controller.Container.GameplaySceneIndex : 0;
}