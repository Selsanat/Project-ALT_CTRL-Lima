using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClockMovement : MonoBehaviour
{
    private Joycon _joycon;

    [SerializeField]
    private float[] _targetsPos = new float[3] {40.0f, 0, -40.0f};

    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float _floatTollerance = 0.1f;

    private int _currentIndex = 0;
    private int _direction = 1;

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

#if UNITY_EDITOR
    [SerializeField]
    private Transform _debugGameObject;
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
        if(_debugGameObject != null)
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

        if(Mathf.Abs(rotationAngle - _targetsPos[_currentIndex]) <= _floatTollerance)
        {
            _currentIndex += _direction;

            if (!_bIsSuceeding)
            {
                _bIsSuceeding = true;
                _onMovementSucceed.Invoke();
            }

            _succeedTimer = 0;

            if (_currentIndex == 0 || _currentIndex == _targetsPos.Length - 1)
            {
                _direction *= -1;
            }
        }

        if (!_bIsSuceeding)
        {
            return;
        }

        _succeedTimer += Time.deltaTime;

        if(_succeedTimer >= _succeedMaxDuration)
        {
            _bIsSuceeding = false;
            _onMovementFailed.Invoke();
        }
    }
}
