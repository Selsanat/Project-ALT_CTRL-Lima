using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Slider _slider;

    private float _timerDuration;

    [SerializeField] private float _succeedFactor = 0.5f;
    [SerializeField] private float _defaultFactor = 1.0f;

    private float _timerFactor;
    private Volume _volume;
    [SerializeField] private UnityEvent _onTimerFinished;

    private bool _bPauseTimer = true;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        RestartTimer(false, _timerDuration);

        _volume = GameObject.FindGameObjectWithTag("GlobalVolume").GetComponent<Volume>();
        _timerFactor = _defaultFactor;
    }

    private void Update()
    {
        _slider.value = Mathf.Clamp(_slider.value, 0.0f, _timerDuration);
        if (_bPauseTimer)
        {
            return;
        }

        _slider.value -= Time.deltaTime * _timerFactor;

        _volume.weight = _slider.value / _slider.maxValue;

        if (_slider.value == 0.0f)
        {
            _onTimerFinished?.Invoke();
        }
    }

    public void RestartTimer(bool playTimer)
    {
        _slider.value = _timerDuration/2;
        PauseTimer(!playTimer);
    }

    public void RestartTimer(bool playTimer, float _timerLength)
    {
        _timerDuration = _timerLength;
        _slider.maxValue = _timerDuration;
        RestartTimer(playTimer);
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
