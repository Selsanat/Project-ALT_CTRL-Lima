using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using static UnityEditor.PlayerSettings;

public enum CharacterType
{
    Client,
    Player,
    Narrator,
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> _characterDatas = new List<CharacterData>();
    private int _characterIndex = -1;

    [SerializeField]
    private GameObject _worldUI;

    [SerializeField]
    private int _characterHierachyIndex = 1;

    private Character _currentCharacter;

    private List<DialogData> _dialogData;
    private DialogData _currentData;
    public static GameManager instance;
    private bool wasChoice = false;
    private Vector3 originalPos1;
    private Vector3 originalPos2;

    [SerializeField] private CharacterBox _characterBox;
    [SerializeField] private ChoiceBox _choiceBox;
    [SerializeField] private DialogBox _playerBox;
    [SerializeField] private DialogBox _narratorBox;

    [SerializeField] private Timer _timer;

    private float _currency = 0;

    [SerializeField] private EndingScreen _endingScreen;

    [SerializeField] private UnityEvent _onDialogFinished;
    [SerializeField] private UnityEvent _onFinishedAllCharacters;

    [SerializeField] private KeyCode _skipDialog = KeyCode.Space;
    [SerializeField] private KeyCode _choiceAInput = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _choiceBInput = KeyCode.RightArrow;

    [SerializeField] public PlayerController _playerController;

    private int _lastDialogIndex;

#if UNITY_EDITOR
    [SerializeField] private int _startCharacterIndex = 0;
#endif

    private void Awake()
    {
        originalPos1 = _choiceBox._choiceA.transform.parent.transform.position;
        originalPos2 = _choiceBox._choiceB.transform.parent.transform.position;
        instance = this;
    }
    private IEnumerator Start()
    {

#if UNITY_EDITOR
        if (_characterBox == null) {Debug.LogError("_characterBox is missing in " + name); yield break;}
        if (_choiceBox == null) {Debug.LogError("_choiceBox is missing in " + name); yield break;}
        if (_playerBox == null) {Debug.LogError("_playerBox is missing in " + name); yield break;}
        if (_timer == null) {Debug.LogError("_timer is missing in " + name); yield break;}
#endif

        yield return new WaitForEndOfFrame();

#if UNITY_EDITOR
        GoToCharacter(_startCharacterIndex);
#else
        GoToCharacter(0);
#endif
    }

    private void Update()
    {
        if (_endingScreen.isActiveAndEnabled)
        {
            return;
        }

#if UNITY_EDITOR
        if(Input.mouseScrollDelta.y != 0)
        {
            int debugIndex = (int)(_lastDialogIndex - Input.mouseScrollDelta.y);

            if (debugIndex < 0)
            {
                bool bIsAtStart = _characterIndex == 0;
                _characterIndex -= 1;
                _characterIndex = Mathf.Clamp(_characterIndex, 0, _characterDatas.Count - 1);

                _currentCharacter.SetData(_characterDatas[_characterIndex]);
                _dialogData = CSVReader.MakeDialogData(_characterDatas[_characterIndex].Dialog);
                debugIndex = bIsAtStart ? 0 : (_dialogData.Count - 1);
            }

            WriteDialog(debugIndex);
            return;
        }
#endif

        if (DialogsController.instance.bIsReadingText)
        {
            if (Input.GetKeyDown(_skipDialog))
            {
                DialogsController.instance.SkipAnimation();
            }

            return;
        }

        int targetIndex = -1;
        int addIndex = -1;

        if (_currentData.bIsChoice)
        {
            if (Input.GetKeyDown(_choiceAInput))
            {
                CanvasGroup cg =   _choiceBox._choiceB.transform.parent.GetComponent<CanvasGroup>();
                cg.DOFade(0, 0.5f).OnComplete(() =>
                {
                    Vector3 pos = _choiceBox._choiceA.transform.parent.transform.position;
                    _choiceBox._choiceA.transform.parent.transform.DOMove(new Vector3(Screen.width / 2, pos.y, pos.z), 0.5f);
                });
                addIndex = _lastDialogIndex;
                targetIndex = _choiceBox.redirectChoiceA;
            }

            else if (Input.GetKeyDown(_choiceBInput))
            {
                CanvasGroup cg = _choiceBox._choiceA.transform.parent.GetComponent<CanvasGroup>();
                cg.DOFade(0, 0.5f).OnComplete(() => {
                    Vector3 pos = _choiceBox._choiceB.transform.parent.transform.position;
                    _choiceBox._choiceB.transform.parent.transform.DOMove(new Vector3(Screen.width / 2, pos.y, pos.z), 0.5f);
                });
                addIndex = _lastDialogIndex + 1;
                targetIndex = _choiceBox.redirectChoiceB;
            }
        }

        else if (Input.GetKeyDown(_skipDialog))
        {
            addIndex = _lastDialogIndex;
            targetIndex = _currentData.redirectIndex;
        }

        if(targetIndex == -1)
        {
            return;
        }

        WriteDialog(targetIndex);
        _timer.AddTime(_dialogData[addIndex].addedTimerValue);
    }

    public IEnumerator playSecondDialog(int dialogIndex)
    {
        yield return new WaitUntil(() => !DialogsController.instance.isReadingText);
        DialogsController.instance.playDialog(_choiceBox._choiceB, _dialogData[dialogIndex + 1].dialog);
        _choiceBox.redirectChoiceB = _dialogData[dialogIndex + 1].redirectIndex;
    }

    public void ResetTextbox()
    {

        CanvasGroup cg1 = _choiceBox._choiceA.transform.parent.GetComponent<CanvasGroup>();
        CanvasGroup cg2 = _choiceBox._choiceB.transform.parent.GetComponent<CanvasGroup>();
        cg1.alpha = 1;
        cg2.alpha = 1;
        _choiceBox._choiceA.transform.parent.transform.position = originalPos1;
        _choiceBox._choiceB.transform.parent.transform.position = originalPos2;
    }
    public void WriteDialog(int dialogIndex)
    {
        if (dialogIndex >= _dialogData.Count)
        {
            _onDialogFinished.Invoke();
            return;
        }

        _lastDialogIndex = dialogIndex;
        _currentData = _dialogData[dialogIndex];

        if (_currentData.bToggleTimer)
        {
            _timer.TogglePause();
        }

        if (_currentData.bIsChoice)
        {
            wasChoice = true;
            _choiceBox.gameObject.SetActive(true);
            _choiceBox._choiceA.text = "";
            _choiceBox._choiceB.text = "";
            DialogsController.instance.playDialog(_choiceBox._choiceA, _currentData.dialog);
            _choiceBox.redirectChoiceA = _currentData.redirectIndex;

            _narratorBox.gameObject.SetActive(false);
            _timer.PauseTimer(false);
            StartCoroutine(playSecondDialog(dialogIndex));
            _characterBox.gameObject.SetActive(false);
            _playerBox.gameObject.SetActive(false);
            return;
        }

        DialogBox targetBox;

        if (_currentData.type == CharacterType.Client)
        {
            targetBox = _characterBox;
            _characterBox.characterName = _currentData.name;

            _characterBox.gameObject.SetActive(true);
            _playerBox.gameObject.SetActive(false);
            _narratorBox.gameObject.SetActive(false);
        }
        else
        {
            _characterBox.gameObject.SetActive(false);

            if(_currentData.type == CharacterType.Narrator)
            {
                _playerBox.gameObject.SetActive(false);
                _narratorBox.gameObject.SetActive(true);
                targetBox = _narratorBox;
            }
            else
            {
                _playerBox.gameObject.SetActive(true);
                _narratorBox.gameObject.SetActive(false);
                targetBox = _playerBox;
            }
        }

        _currentCharacter.SetEmotion(_currentData.emotion);
        DialogsController.instance.playDialog(targetBox._dialogText, _currentData.dialog);
        if (wasChoice) wasChoice = false;
        else _choiceBox.gameObject.SetActive(false);

    }

    public void GoToCharacter(int characterIndex)
    {
        _characterIndex = characterIndex;
        if (_characterIndex >= _characterDatas.Count)
        {
            _onFinishedAllCharacters.Invoke();
            return;
        }

        _playerController.ResetClock();
        _timer.gameObject.SetActive(true);

        if (_currentCharacter != null)
        {
            Destroy(_currentCharacter.gameObject);
        }

        _currentCharacter = Instantiate(_characterDatas[_characterIndex].Character, _worldUI.transform);
        _currentCharacter.transform.SetSiblingIndex(_characterHierachyIndex);
        _currentCharacter.SetData(_characterDatas[_characterIndex]);
        Vector3 pos = _currentCharacter.transform.position;
        _characterBox.gameObject.SetActive(false);
        _playerBox.gameObject.SetActive(false);
        ResetTextbox();
        _choiceBox.gameObject.SetActive(false);
        _narratorBox.gameObject.SetActive(false);
        _currentCharacter.transform.position = new Vector3(pos.x + 20, pos.y, pos.z);
        _dialogData = CSVReader.MakeDialogData(_characterDatas[_characterIndex].Dialog);
        _currentCharacter.transform.DOJump(pos, 1, 6, 3).SetEase(Ease.InSine).OnComplete(() =>
        {
            _dialogData = CSVReader.MakeDialogData(_characterDatas[_characterIndex].Dialog);
            _timer.RestartTimer(true, _characterDatas[_characterIndex].CharacterTimerLenght);
            WriteDialog(0);
            _endingScreen.gameObject.SetActive(false);
        });
    }

    public void ResetCurrentCharacter()
    {
        GoToCharacter(_characterIndex);
    }

    public void ToNextCharacter()
    {
        GoToCharacter(_characterIndex + 1);
    }

    public void EndClient(bool bVictory)
    {
        if (bVictory)
        {
            _currency += _timer.GetTimerValueInPercent();
        }

        _characterBox.gameObject.SetActive(false);
        _playerBox.gameObject.SetActive(false);
        _choiceBox.gameObject.SetActive(false);
        _timer.gameObject.SetActive(false);
        _narratorBox.gameObject.SetActive(false);

        _endingScreen.gameObject.SetActive(true);
        _endingScreen.DisplayResults(bVictory, _currency);
    }
}
