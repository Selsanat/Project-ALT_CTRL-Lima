using UnityEngine;
using UnityEngine.UI;

public class PendulumMovement : MonoBehaviour
{
    [SerializeField] private Image _pendulum;

    [SerializeField]
    [Range(0, 90.0f)]
    private float _maxRotation = 45.0f;

    [SerializeField]
    private float _rotationSpeed = 1.0f;
    private int _rotationDirection = 1;

    [SerializeField]
    private AnimationCurve _rotationCurve;

    private float _alpha;

    private float currentZRotation;
    public float TargetRotation => currentZRotation;

    private void Update()
    {
        _alpha += (Time.deltaTime * _rotationSpeed * _rotationDirection);
        _alpha = Mathf.Clamp(_alpha, 0.0f, 1.0f);

        currentZRotation = Mathf.Lerp(-_maxRotation, _maxRotation, _rotationCurve.Evaluate(_alpha));

        _pendulum.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, currentZRotation);

        if(_alpha == 0.0f || _alpha == 1.0f)
        {
            _rotationDirection *= -1;
        }
    }
}
