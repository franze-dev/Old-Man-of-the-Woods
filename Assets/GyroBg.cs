using UnityEngine;
using UnityEngine.InputSystem;

public class GyroBg : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _sensitivity = 0.2f;
    [SerializeField] private float _shiftModifier = 1f;

    [Header("Clamp Settings")]
    [SerializeField] private float _minX = -3f;
    [SerializeField] private float _maxX = 3f; 

    private UnityEngine.InputSystem.Gyroscope _gyro;

    private void Start()
    {
        _gyro = UnityEngine.InputSystem.Gyroscope.current;

        if (_gyro == null)
        {
            Debug.LogWarning("No gyroscope found.");
            return;
        }

        InputSystem.EnableDevice(_gyro);
    }

    private void Update()
    {
        if (_gyro == null) return;

        Vector3 rate = _gyro.angularVelocity.ReadValue();

        float delta = rate.y * _sensitivity * _shiftModifier;

        transform.Translate(delta, 0f, 0f);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _minX, _maxX);
        transform.position = pos;
    }
}