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

    [SerializeField] private UnityEvent _onDialogFinished;
    [SerializeField] private UnityEvent _onFinishedAllCharacters;

#if UNITY_EDITOR
    private int _lastDialogIndex;
#endif

    private void Start()
    {
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
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                targetIndex = _choiceBox.redirectChoiceA;
            }

            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                targetIndex = _choiceBox.redirectChoiceB;
            }
        }

        else if (Input.GetKeyUp(KeyCode.Mouse0))
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
            ToNextCharacter();
            _onDialogFinished.Invoke();
            return;
        }

#if UNITY_EDITOR
        _lastDialogIndex = dialogIndex;
#endif

        _currentData = _dialogData[dialogIndex];

        if (_currentData.bIsChoice)
        {
            _choiceBox.choiceA = _currentData.dialog;
            _choiceBox.redirectChoiceA = _currentData.redirectIndex;

            _choiceBox.choiceB = _dialogData[dialogIndex + 1].dialog;
            _choiceBox.redirectChoiceB = _dialogData[dialogIndex + 1].redirectIndex;

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

            _characterBox.gameObject.SetActive(false);
            _playerBox.gameObject.SetActive(true);
        }

        _character.SetEmotion(_currentData.emotion);
        targetBox.dialog = _currentData.dialog;
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
