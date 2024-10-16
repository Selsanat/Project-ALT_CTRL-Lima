using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Slider _slider;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _timerDuration = 50f;

    [SerializeField] private float _succeedFactor = 0.5f;
    [SerializeField] private float _defaultFactor = 1.0f;

    private float _timerFactor;

    [SerializeField] private UnityEvent _onTimerGreaterThanHalf;
    [SerializeField] private UnityEvent _onTimerLessThanHalf;
    [SerializeField] private UnityEvent _onTimerFinished;

    private bool _bReachHalf => (_slider.value > _timerDuration/2);

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _timerDuration;
        _slider.value = _timerDuration/2;

        _timerFactor = _defaultFactor;
    }

    private void Update()
    {
        _slider.value -= Time.deltaTime * _timerFactor;
        _slider.value = Mathf.Clamp(_slider.value, 0.0f, _timerDuration);

        if (!_bReachHalf)
        {
            _onTimerLessThanHalf.Invoke();
        }

        if (_slider.value == 0.0f)
        {
            _onTimerFinished?.Invoke();
        }
    }

    public void AddTimer(float timeToAdd)
    {
        _slider.value += timeToAdd;
        _slider.value = Mathf.Clamp(_slider.value, 0.0f, _timerDuration);

        if (_bReachHalf)
        {
            _onTimerGreaterThanHalf?.Invoke();
        }
    }

    public void ToggleTimerFactor()
    {
        _timerFactor = _timerFactor == _defaultFactor ? _succeedFactor : _defaultFactor;
    }
}
