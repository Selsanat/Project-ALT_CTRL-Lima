using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Joycon _joycon;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    [Range(0f, 100.0f)]
    private float _addedTimerInPercent = 50.0f;

    private bool _bCanAddTime = true;

    private void Start()
    {
        if (JoyconManager.Instance.j.Count == 0)
        {
            return;
        }

        _joycon = JoyconManager.Instance.j[0];
    }

    private void Update()
    {
        if(_joycon == null)
        {
            return;
        }

        if (!_bCanAddTime)
        {
            return;
        }

        if (_joycon.GetButtonDown(Joycon.Button.SHOULDER_1))
        {
            _timer.AddTimeInPercent(_addedTimerInPercent);
        }
    }

    public void ResetClock()
    {
        _bCanAddTime = true;
    }
}
