using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingScreen : MenuManager
{
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private TextMeshProUGUI _currencyDisplay;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] private string _victoryText = "Victory";
    [SerializeField] private string _failedText = "Failed";

    [SerializeField] private RectTransform _hypnotizedSprite;
    [SerializeField] private float _rotationSpeed = 10.0f;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float rotation = _hypnotizedSprite.transform.eulerAngles.z;
        rotation += Time.deltaTime * _rotationSpeed;

        _hypnotizedSprite.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotation));
    }

    public void DisplayResults(bool bVictory, float currency)
    {
        _stateText.text = bVictory ? _victoryText : _failedText;
        _currencyDisplay.text = Mathf.CeilToInt(currency).ToString();
    }

    public void Continue()
    {
        bool bVictory = _stateText.text == _victoryText;

        if (bVictory)
        {
            _gameManager.ToNextCharacter();
        }
        else
        {
            _gameManager.ResetCurrentCharacter();
        }
    }
}
