using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _loadBar;

    private void Awake()
    {
        if (_loadBar == null)
            Debug.LogError("No load bar found");

        _loadBar.fillAmount = 0;

        ServiceProvider.SetService(this);

        EventProvider.Subscribe<ILoadEvent>(LoadBar);
        EventProvider.Subscribe<IResetLoadEvent>(ResetBar);
    }

    private void OnDestroy()
    {
        ServiceProvider.SetService<LoadingScreen>(null);
    }

    private void LoadBar(ILoadEvent load)
    {
        gameObject.SetActive(true);

        var progress = Mathf.Clamp01(load.Progress / 0.9f);

        _loadBar.fillAmount = progress;
    }

    private void ResetBar(IResetLoadEvent load)
    {
        _loadBar.fillAmount = 0;
        gameObject.SetActive(false);
    }
}

public interface ILoadEvent : IEvent
{
    float Progress { get; set; }
}

public class LoadEvent : ILoadEvent
{
    public float Progress { get; set; }
    
    public LoadEvent(float progress)
    {
        Progress = progress;
    }

}

public interface IResetLoadEvent : IEvent
{
}

public class ResetLoadEvent : IResetLoadEvent
{
}