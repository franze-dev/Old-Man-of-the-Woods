using TMPro;
using UnityEngine;

public class LevelTimerUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private LevelManager _levelManager;

    private void Start()
    {
        ServiceProvider.TryGetService(out _levelManager);

        _timerText.text = ((int)_levelManager.MaxTimer).ToString();
    }

    private void Update()
    {
        if (!_levelManager.TimerStarted)
        {
            _timerText.text = ((int)_levelManager.MaxTimer).ToString();
            return;
        }

        _timerText.text = ((int)_levelManager.Timer).ToString();
    }
}
