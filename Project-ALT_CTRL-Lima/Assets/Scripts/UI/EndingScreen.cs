using TMPro;
using UnityEngine;

public class EndingScreen : MenuManager
{
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private TextMeshProUGUI _currencyDisplay;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] private string _victoryText = "Victory";
    [SerializeField] private string _failedText = "Failed";

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void DisplayResults(bool bVictory, float currency)
    {
        _stateText.text = bVictory ? _victoryText : _failedText;
        _currencyDisplay.text = currency.ToString();
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
