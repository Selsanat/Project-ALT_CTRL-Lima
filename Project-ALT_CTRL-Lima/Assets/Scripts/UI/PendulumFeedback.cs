using UnityEngine;
using UnityEngine.UI;

public class PendulumFeedback : MonoBehaviour
{
    private Image _currentImage;

    [SerializeField] private Sprite _succeedImage;
    [SerializeField] private Sprite _failImage;

    private void Start()
    {
        _currentImage = GetComponent<Image>();
        _currentImage.sprite = _failImage;
    }

    public void SetSucceedImage()
    {
        _currentImage.sprite = _succeedImage;
    }

    public void SetFailImage()
    {
        _currentImage.sprite = _failImage;
    }
}
