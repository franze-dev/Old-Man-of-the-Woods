using UnityEngine;

public class SpawnsSetup : MonoBehaviour
{
    [SerializeField] private GameObject _leftSpawn;
    [SerializeField] private GameObject _rightSpawn;
    [SerializeField] private GameObject _leftLimit;
    [SerializeField] private GameObject _rightLimit;
    [SerializeField] private GameObject _pivot;
    [SerializeField] private float _offsetXSpawn = 13f;
    [SerializeField] private float _offsetXLimit;

    private void Awake()
    {
        if (_offsetXLimit == 0)
            _offsetXLimit = _offsetXSpawn + 3;

        _leftSpawn.transform.position = new Vector2(_pivot.transform.position.x - _offsetXSpawn, _pivot.transform.position.y);
        _rightSpawn.transform.position = new Vector2(_pivot.transform.position.x + _offsetXSpawn, _pivot.transform.position.y);

        _leftLimit.transform.position = new Vector2(_pivot.transform.position.x - _offsetXLimit, _pivot.transform.position.y);
        _rightSpawn.transform.position = new Vector2(_pivot.transform.position.x + _offsetXLimit, _pivot.transform.position.y);
    }
}
