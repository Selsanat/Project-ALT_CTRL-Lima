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

    [SerializeField] private UnityEvent _onTimerFinished;

    private bool _bPauseTimer = false;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _timerDuration;
        _slider.value = _timerDuration/2;

        _timerFactor = _defaultFactor;
    }

    private void Update()
    {
        if (_bPauseTimer)
        {
            return;
        }

        _slider.value -= Time.deltaTime * _timerFactor;
        _slider.value = Mathf.Clamp(_slider.value, 0.0f, _timerDuration);

        if (_slider.value == 0.0f)
        {
            _onTimerFinished?.Invoke();
        }
    }

    public void RestartTimer()
    {
        _slider.value = _timerDuration/2;
    }

    public void AddTime(float timeToAdd)
    {
        _slider.value += timeToAdd;
        _slider.value = Mathf.Clamp(_slider.value, 0.0f, _timerDuration);
    }

    public void AddTimeInPercent(float percentToAdd)
    {
        AddTime(percentToAdd * 100.0f / _timerDuration);
    }

    public void ToggleTimerFactor()
    {
        _timerFactor = _timerFactor == _defaultFactor ? _succeedFactor : _defaultFactor;
    }

    public void PauseTimer(bool bPause)
    {
        _bPauseTimer = bPause;
    }

    public float GetTimerValueInPercent()
    {
        return _slider.value * 100.0f / _timerDuration;
    }
}
