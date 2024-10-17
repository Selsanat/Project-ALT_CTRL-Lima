using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private List<CharacterData> _characterDatas = new List<CharacterData>();
    private int _characterIndex = -1;

    [SerializeField]
    private Character _character;

    public List<DialogueData> _dialogueData;
    private DialogueData _currentData;

    [SerializeField]
    private DialogueBox _dialogueBox;

    [SerializeField]
    private ChoiceBox _choiceBox;

    private void Start()
    {
        ToNextCharacter();
    }

    private void Update()
    {
        int targetIndex = -1;

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

        WriteDialogue(targetIndex);
    }

    public void WriteDialogue(int dialogueIndex)
    {
        if (dialogueIndex >= _dialogueData.Count)
        {
            return;
        }

        _currentData = _dialogueData[dialogueIndex];

        if (_currentData.bIsChoice)
        {
            _choiceBox.choiceA = _currentData.dialogue;
            _choiceBox.redirectChoiceA = _currentData.redirectIndex;

            _choiceBox.choiceB = _dialogueData[dialogueIndex + 1].dialogue;
            _choiceBox.redirectChoiceB = _dialogueData[dialogueIndex + 1].redirectIndex;

            _dialogueBox.gameObject.SetActive(false);
            _choiceBox.gameObject.SetActive(true);
            return;
        }

        _dialogueBox.characterName = _currentData.name;
        _dialogueBox.dialogue = _currentData.dialogue;
        _character.SetEmotion(_currentData.emotion);

        _dialogueBox.gameObject.SetActive(true);
        _choiceBox.gameObject.SetActive(false);
    }

    public void ToNextCharacter()
    {
        _characterIndex++;
        _dialogueData = CSVReader.MakeDialogueData(_characterDatas[_characterIndex].Dialogues);
        _character.SetData(_characterDatas[_characterIndex]);

        WriteDialogue(0);
    }
}
