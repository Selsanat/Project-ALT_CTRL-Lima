using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Timer : MonoBehaviour
{
    private float _timerDuration;

    [SerializeField] private float _succeedFactor = 0.5f;
    [SerializeField] private float _defaultFactor = 1.0f;

    private float _timerFactor;
    private Volume _volume;
    [SerializeField] private UnityEvent _onTimerFinished;

    private bool _bPauseTimer = true;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    [Tooltip("Percentage")]
    private float _startValue = 0.5f;

    public float _value;

    [SerializeField]
    private RectTransform _maskTransform;

    private float MaxSize;

    [SerializeField]
    private UnityEvent<float> _onValueUpdate;

    private void Start()
    {
        _volume = GameObject.FindGameObjectWithTag("GlobalVolume").GetComponent<Volume>();

        _onValueUpdate.AddListener(UpdateMaskPos);
        _onValueUpdate.AddListener(UpdateVolume);

        RestartTimer(false, _timerDuration);
        _timerFactor = _defaultFactor;

        MaxSize = GetComponent<RectTransform>().sizeDelta.y;
    }

    private void Update()
    {
        if (_bPauseTimer)
        {
            return;
        }

        _value -= Time.deltaTime * _timerFactor;
        _value = Mathf.Clamp(_value, 0.0f, _timerDuration);

        _onValueUpdate.Invoke(_value);

        _volume.weight = _value / _timerDuration;

        if (_value == 0.0f)
        {
            _onTimerFinished?.Invoke();
        }
    }

    public void RestartTimer(bool playTimer)
    {
        _value = _startValue * _timerDuration;
        _onValueUpdate.Invoke(_value);
        PauseTimer(!playTimer);
    }

    public void RestartTimer(bool playTimer, float _timerLength)
    {
        _timerDuration = _timerLength;
        RestartTimer(playTimer);
    }

    public void AddTime(float timeToAdd)
    {
        _value += timeToAdd;
        _value = Mathf.Clamp(_value, 0.0f, _timerDuration);
        _onValueUpdate.Invoke(_value);

        if (_value == 0.0f)
        {
            _onTimerFinished?.Invoke();
        }
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

    public void TogglePause()
    {
        _bPauseTimer = !_bPauseTimer;
    }

    public float GetTimerValueInPercent()
    {
        return _value * 100.0f / _timerDuration;
    }

    private void UpdateMaskPos(float alpha)
    {
        alpha = Mathf.InverseLerp(0.0f, _timerDuration, alpha);
        float TargetPos = Mathf.Lerp(0.0f, MaxSize, alpha);

        _maskTransform.anchoredPosition = new Vector2(_maskTransform.anchoredPosition.x, TargetPos);
    }

    private void UpdateVolume(float alpha)
    {
        if (alpha == 0) _volume.weight = 0.75f;
        else _volume.weight = Mathf.Clamp(alpha / _timerDuration-0.5f,0,1);
    }
}
