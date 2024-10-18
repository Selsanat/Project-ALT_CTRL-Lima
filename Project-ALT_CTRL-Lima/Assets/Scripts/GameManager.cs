using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Timer _timer;

    private float _currency = 0;

    public void ComputeCurrency()
    {
        _currency += _timer.GetTimerValueInPercent();
    }
}
