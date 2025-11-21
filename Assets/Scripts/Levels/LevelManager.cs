using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _finalLevel = 3;
    [SerializeField] private float _levelTimer = 60f;

    private void Start()
    {
        EventTriggerer.Trigger<ILevelUpEvent>(new LevelUpEvent(_currentLevel));
    }
}
