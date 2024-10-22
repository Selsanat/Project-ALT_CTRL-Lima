using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class EndingScreen : MenuManager
{
    [SerializeField] private TextMeshProUGUI _stateText;
    [SerializeField] private TextMeshProUGUI _currencyDisplay;
    [SerializeField] private TextMeshProUGUI _continueDisplay;

    [SerializeField] private GameManager _gameManager;

    [SerializeField] private string _victoryText = "Victory";
    [SerializeField] private string _failedText = "Failed";

    [SerializeField] private Image _continueImage;
    [SerializeField] private Sprite _continueSprite;
    [SerializeField] private Sprite _retrySprite;

    [SerializeField] private string _continueText = "Continue";
    [SerializeField] private string _retryText = "Retry";

    [SerializeField] private Color _hypnotizedColor;
    [SerializeField] private Color _charlatanColor;
    [SerializeField] private Color _textHypnotizedColor;
    [SerializeField] private Color _textcharlatanColor;

    [SerializeField] private Image _hypnotizedSprite;
    [SerializeField] private float _rotationSpeed = 10.0f;

    [SerializeField] private Image _charlatanSprite;
    [SerializeField] private float _translationSpeed = 10.0f;

    private float _spriteHeight;

    private bool _bVictory;

    private void Start()
    {
        gameObject.SetActive(false);

        _spriteHeight = _charlatanSprite.rectTransform.sizeDelta.y;
    }

    private void Update()
    {
        if (_bVictory)
        {
            float rotation = _hypnotizedSprite.transform.eulerAngles.z;
            rotation += Time.deltaTime * _rotationSpeed;

            _hypnotizedSprite.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotation));
        }
        else
        {
            float position = _charlatanSprite.transform.localPosition.y;
            position -= Time.deltaTime * _translationSpeed;

            if(position < -_spriteHeight)
            {
                position = 0;
            }

            _charlatanSprite.transform.localPosition = new Vector3(0.0f, position, 0.0f);
        }
    }

    public void DisplayResults(bool bVictory, float currency)
    {
        _bVictory = bVictory;
        _stateText.text = _bVictory ? _victoryText : _failedText;
        _stateText.color = _bVictory ? _hypnotizedColor : _charlatanColor;

        _continueDisplay.text = _bVictory ? _continueText : _retryText;
        _continueImage.sprite = _bVictory ? _continueSprite : _retrySprite;

        _hypnotizedSprite.gameObject.SetActive(_bVictory);
        _charlatanSprite.gameObject.SetActive(!_bVictory);

        _currencyDisplay.text = Mathf.CeilToInt(currency).ToString();
        _currencyDisplay.color = _bVictory ? _textHypnotizedColor : _textcharlatanColor;
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
