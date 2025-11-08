public interface IDatosEscena
{
    /// <summary>
    /// Saves the index of the scene
    /// </summary>
    static int Index { get; }
}

public class MenuSceneData : IDatosEscena
{
    public static int Index => ServiceProvider.TryGetService<SceneFlowManager>(out var controller) ? 
                               controller.Container.EscenaMenuIndice : 0;
}

public class GameplaySceneData : IDatosEscena
{
    public static int Index => ServiceProvider.TryGetService<SceneFlowManager>(out var controller) ?
                               controller.Container.EscenaJuegoIndice : 0;
}