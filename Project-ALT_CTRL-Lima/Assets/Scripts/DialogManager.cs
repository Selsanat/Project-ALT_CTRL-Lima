using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterType
{
    Client,
    Player,
    Narrator,
}

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> _characterDatas = new List<CharacterData>();
    private int _characterIndex = -1;

    [SerializeField]
    private Character _character;

    private List<DialogData> _dialogData;
    private DialogData _currentData;

    [SerializeField] private CharacterBox _characterBox;
    [SerializeField] private ChoiceBox _choiceBox;
    [SerializeField] private PlayerBox _playerBox;

    [SerializeField] private Timer _timer;

    [SerializeField] private UnityEvent _onDialogFinished;
    [SerializeField] private UnityEvent _onFinishedAllCharacters;

    [SerializeField] private KeyCode _skipDialog = KeyCode.Space;
    [SerializeField] private KeyCode _choiceAInput = KeyCode.LeftArrow;
    [SerializeField] private KeyCode _choiceBInput = KeyCode.RightArrow;

#if UNITY_EDITOR
    private int _lastDialogIndex;
#endif

    private IEnumerator Start()
    {

#if UNITY_EDITOR
        if (_characterBox == null) {Debug.LogError("_characterBox is missing in " + name); yield break;}
        if (_choiceBox == null) {Debug.LogError("_choiceBox is missing in " + name); yield break;}
        if (_playerBox == null) {Debug.LogError("_playerBox is missing in " + name); yield break;}
        if (_timer == null) {Debug.LogError("_timer is missing in " + name); yield break;}
#endif

        yield return new WaitForEndOfFrame();

        ToNextCharacter();
    }

    private void Update()
    {
        int targetIndex = -1;

#if UNITY_EDITOR
        if(Input.mouseScrollDelta.y != 0)
        {
            targetIndex = (int)(_lastDialogIndex - Input.mouseScrollDelta.y);

            if (targetIndex < 0)
            {
                bool bIsAtStart = _characterIndex == 0;
                _characterIndex -= 1;
                _characterIndex = Mathf.Clamp(_characterIndex, 0, _characterDatas.Count - 1);

                _character.SetData(_characterDatas[_characterIndex]);
                _dialogData = CSVReader.MakeDialogData(_characterDatas[_characterIndex].Dialog);
                targetIndex = bIsAtStart ? 0 : (_dialogData.Count - 1);
            }
        }
#endif

        if (_currentData.bIsChoice)
        {
            if (Input.GetKeyUp(_choiceAInput))
            {
                targetIndex = _choiceBox.redirectChoiceA;
            }

            else if (Input.GetKeyUp(_choiceBInput))
            {
                targetIndex = _choiceBox.redirectChoiceB;
            }
        }

        else if (Input.GetKeyUp(_skipDialog))
        {
            targetIndex = _currentData.redirectIndex;
        }

        if(targetIndex == -1)
        {
            return;
        }

        WriteDialog(targetIndex);
    }

    public void WriteDialog(int dialogIndex)
    {
        if (dialogIndex >= _dialogData.Count)
        {
            _onDialogFinished.Invoke();
            return;
        }

#if UNITY_EDITOR
        _lastDialogIndex = dialogIndex;
#endif

        _currentData = _dialogData[dialogIndex];

        if (_currentData.bIsChoice)
        {
            DialogsController.instance.playDialog(_choiceBox._choiceA, _currentData.dialog);
            _choiceBox.redirectChoiceA = _currentData.redirectIndex;

            DialogsController.instance.playDialog(_choiceBox._choiceB, _dialogData[dialogIndex + 1].dialog);
            _choiceBox.redirectChoiceB = _dialogData[dialogIndex + 1].redirectIndex;

            _timer.PauseTimer(false);
            _characterBox.gameObject.SetActive(false);
            _playerBox.gameObject.SetActive(false);
            _choiceBox.gameObject.SetActive(true);
            return;
        }

        DialogBox targetBox;

        if (_currentData.type == CharacterType.Client)
        {
            targetBox = _characterBox;
            _characterBox.characterName = _currentData.name;

            _characterBox.gameObject.SetActive(true);
            _playerBox.gameObject.SetActive(false);
        }
        else
        {
            targetBox = _playerBox;
            _playerBox.SetDialogBox(_currentData.type);

            _timer.PauseTimer(_currentData.type == CharacterType.Narrator);

            _characterBox.gameObject.SetActive(false);
            _playerBox.gameObject.SetActive(true);
        }

        _character.SetEmotion(_currentData.emotion);
        DialogsController.instance.playDialog(targetBox._dialogText, _currentData.dialog);
        _choiceBox.gameObject.SetActive(false);
    }

    public void ToNextCharacter()
    {
        _characterIndex++;
        if (_characterIndex >= _characterDatas.Count)
        {
            _onFinishedAllCharacters.Invoke();
            return;
        }

        _character.SetData(_characterDatas[_characterIndex]);

#if UNITY_EDITOR
        _lastDialogIndex = 0;

        if (_characterDatas[_characterIndex].Dialog == null)
        {
            Debug.LogWarning(_characterDatas[_characterIndex].name + " has null dialog data");
            return;
        }
#endif

        _dialogData = CSVReader.MakeDialogData(_characterDatas[_characterIndex].Dialog);

        WriteDialog(0);
    }
}
