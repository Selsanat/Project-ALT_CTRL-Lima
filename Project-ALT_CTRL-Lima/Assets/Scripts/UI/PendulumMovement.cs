using UnityEngine;
using UnityEngine.UI;

public class PendulumMovement : MonoBehaviour
{
    [SerializeField] private Image _pendulum;

    [SerializeField]
    [Range(0, 90.0f)]
    private float _maxRotation = 45.0f;

    [SerializeField]
    private float _rotationSpeed = 100.0f;
    private int _rotationDirection = 1;

    private float _currentZ;

    private void Update()
    {
        _currentZ += (Time.deltaTime * _rotationDirection * _rotationSpeed);
        _pendulum.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, _currentZ);

        if ((_currentZ >= _maxRotation && _rotationDirection == 1) || (_currentZ <= -_maxRotation && _rotationDirection == -1))
        {
            _rotationDirection *= -1;
        }
    }
}
