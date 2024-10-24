using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClockMovement : MonoBehaviour
{
    private Joycon _joycon;

    [Header("Events")]

    [SerializeField] private UnityEvent _onMovementSucceed;
    [SerializeField] private UnityEvent _onMovementFailed;

    [Header("GameplayTollerance")]

    [SerializeField]
    [Range(0.0f, 50.0f)]
    private float _floatTollerance = 0.1f;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _succeedMaxDuration = 2.5f;
    private float _succeedTimer;

    private bool _bIsSuceeding = false;

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _accelerationDelay = 1.0f;
    private float _accelerationTimer;

    private bool _bIsMoving = false;

    [SerializeField]
    private float _maxDistance = 5.0f;

    [SerializeField]
    private float _targetAcceleration = 1.5f;

    [Header("JoyCon Debug")]

    [SerializeField]
    [Tooltip("Timer in seconds")]
    private float _recenterDuration = 60.0f;
    private float _recenterTimer;

    [SerializeField]
    private int _resetCount = 100;
    private int _resetNbr = 0;

    [SerializeField]
    private bool _bInvertJoyCon = true;

    [Header("References")]

    [SerializeField]
    private PendulumMovement _pendulum;

#if UNITY_EDITOR
    [Header("EditorOnly")]
    [SerializeField]
    private Transform _debugGameObject;
    private float _debugRotation;

    private float _debugDistance;

    [SerializeField]
    private bool _bFullDebug = true;
#endif

    private IEnumerator Start()
    {
        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        GameManager.instance._playerController = playerController;
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetJoycon();
        }

        if (_joycon.GetAccel() == Vector3.zero)
        {
#if UNITY_EDITOR
            Debug.LogWarning("joyCon disconnected");
#endif
            _resetNbr++;
            if(_resetNbr >= _resetCount)
            {
                _resetNbr = 0;
                ResetJoycon();
            }
        }

        _recenterTimer += Time.deltaTime;

        if(_recenterTimer >= _recenterDuration)
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

        rotationAngle *= _bInvertJoyCon ? -1 : 1;

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

#if UNITY_EDITOR
        _debugDistance = distance;
#endif

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

    private void ResetJoycon()
    {
        JoyconManager.Instance.Connect(false);
        _joycon = JoyconManager.Instance.j[0];
        _joycon.Recenter();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if(_joycon == null)
        {
            return;
        }

        GUILayout.TextArea("Succeed: " + _bIsSuceeding.ToString());

        if (!_bFullDebug)
        {
            return;
        }

        GUILayout.Space(5);
        GUILayout.TextArea(_debugRotation.ToString());
        GUILayout.TextArea(_pendulum.TargetRotation.ToString());
        GUILayout.TextArea(_debugDistance.ToString());
        GUILayout.Space(5);
        GUILayout.TextArea(_joycon.GetAccel().ToString());
        GUILayout.Space(5);
        GUILayout.TextArea(_bIsMoving.ToString());
    }
#endif
}
