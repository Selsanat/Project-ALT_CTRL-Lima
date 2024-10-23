using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClockMovement : MonoBehaviour
{
    private Joycon _joycon;

    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float _floatTollerance = 0.1f;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _succeedMaxDuration = 2.5f;
    private float _succeedTimer;

    [SerializeField] private UnityEvent _onMovementSucceed;
    [SerializeField] private UnityEvent _onMovementFailed;

    private bool _bIsSuceeding = false;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _resetDuration = 60.0f;
    private float _recenterTimer;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _accelerationDelay = 1.0f;
    private float _accelerationTimer;

    private bool _bIsMoving = false;

    [SerializeField]
    private PendulumMovement _pendulum;

    [SerializeField]
    private float _maxDistance = 5.0f;

    [SerializeField]
    private float _targetAcceleration = 1.5f;

#if UNITY_EDITOR
    [SerializeField]
    private Transform _debugGameObject;
    private float _debugRotation;
#endif

    private IEnumerator Start()
    {
        if(JoyconManager.Instance.j.Count == 0)
        {
            yield break;
        }

        _joycon = JoyconManager.Instance.j[0];

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // recenter the joycon after 2 frames
        _joycon.Recenter();
    }

    private void Update()
    {
        if (_joycon == null)
        {
            return;
        }

        _recenterTimer += Time.deltaTime;

        if(_recenterTimer >= _resetDuration)
        {
            _joycon.Recenter();
            _recenterTimer = 0;
        }

        float rotationAngle = _joycon.GetVector().eulerAngles.y + 180.0f;

#if UNITY_EDITOR
        if (_debugGameObject != null)
        {
            _debugGameObject.rotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
        }
#endif

        while (rotationAngle > 180.0f)
        {
            rotationAngle -= 360.0f;
        }

        while (rotationAngle < -180.0f)
        {
            rotationAngle += 360.0f;
        }

#if UNITY_EDITOR
        _debugRotation = rotationAngle;
#endif

        if (_bIsSuceeding)
        {
            _succeedTimer += Time.deltaTime;

            if (_succeedTimer >= _succeedMaxDuration)
            {
                _bIsSuceeding = false;
                _onMovementFailed.Invoke();
            }
        }

        _accelerationTimer += Time.deltaTime;

        if (_joycon.GetAccel().x >= _targetAcceleration)
        {
            _bIsMoving = true;
            _accelerationTimer = 0;
        }

        else if (_accelerationTimer > _accelerationDelay)
        {
            _bIsMoving = false;
            _accelerationTimer = 0;
        }

        if (!_bIsMoving)
        {
            return;
        }

        float distance = Mathf.Abs(rotationAngle - _pendulum.TargetRotation);

        if (distance > _maxDistance)
        {
            if (_bIsSuceeding)
            {
                _bIsSuceeding = false;
                _onMovementFailed.Invoke();
            }

            return;
        }

        if (distance <= _floatTollerance)
        {
            if (!_bIsSuceeding)
            {
                _bIsSuceeding = true;
                _onMovementSucceed.Invoke();
            }

            _succeedTimer = 0;
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.TextArea("Succeed: " + _bIsSuceeding.ToString());
    }
#endif
}
