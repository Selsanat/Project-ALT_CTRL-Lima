using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class DialogsController : MonoBehaviour
{
    [SerializeField] TMP_InputField dialogInput;
    [SerializeField] int charactersPerSecond = 1;
    [SerializeField] TMP_Text _dialogText;
    private float _readCharacterOffset = 0;
    private int _readMaxCharacters = 0;
    private TMP_TextInfo _textInfo;
    private bool isReadingText = false;
    private ProcessedText _currentProcessedText;

    private class ProcessedText
    {
        public string processedText = "";
        public Dictionary<int, List<TextCommand>> commands = new Dictionary<int, List<TextCommand>>();
    }
    private void Update()
    {
        _UpdateReadText();
    }
    public void GoToEnd()
    {
        _dialogText.maxVisibleCharacters = _readMaxCharacters;
        isReadingText = false;
    }
    private void _UpdateReadText()
    {
        if (!isReadingText) return;
        int nextLetterCalcul = (int)(_readCharacterOffset + charactersPerSecond * Time.deltaTime);
        bool hasSecondPassed = (int)_readCharacterOffset + 1 == nextLetterCalcul; // Check if a second has passed
        if (hasSecondPassed)
        {
            _currentProcessedText.commands[nextLetterCalcul-1].ForEach(command => {
                command.OnEnter();
                });
        }
        _readCharacterOffset += charactersPerSecond * Time.deltaTime;
        _dialogText.maxVisibleCharacters = (int)_readCharacterOffset;
        if (_readCharacterOffset >= _readMaxCharacters) GoToEnd();
    }
    public void ReadText()
    {
        _currentProcessedText = _GenerateCommands(dialogInput.text);
        _dialogText.text = _currentProcessedText.processedText;
        _textInfo = _dialogText.textInfo;
        _dialogText.ForceMeshUpdate();
        _readCharacterOffset = 0;
        _readMaxCharacters = _dialogText.GetParsedText().Length;
        _dialogText.maxVisibleCharacters = 0;
        isReadingText = true;
        initText();
    }

    public void initText()
    {
        foreach (KeyValuePair<int, List<TextCommand>> command in _currentProcessedText.commands)
        {
            command.Value.ForEach(c =>
            {
                c.Init(_dialogText, command.Key);
            });
        }
    }
    public void showDialog()
    {
        ReadText();
    }
    private IEnumerator ShowDialog()
    {
        _textInfo = _dialogText.textInfo;
        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            int meshIndex = _dialogText.textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = _dialogText.textInfo.characterInfo[i].vertexIndex;

            if (!char.IsWhiteSpace(_dialogText.textInfo.characterInfo[i].character)){


                _dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private static ProcessedText _GenerateCommands(string text)
    {
        string modifiedText = text;
        ProcessedText result = new ProcessedText();
        TextCommandsFactory factory = new TextCommandsFactory();
        List<string> activeCommands = new List<string>();
        bool isInsideTag = false;
        int tagOffset = 0;
        string tag = "";
        for (int i = 0; i < text.Length; ++i)
        {
            result.commands[i] = new List<TextCommand>();
            char character = text[i];
            switch (character)
            {
                case '<':
                    {
                    isInsideTag = true;
                        break;
                    };
            }
            if (isInsideTag)
            {
                tagOffset++;
                tag += character;
                if (character == '>') 
                {
                    isInsideTag = false;
                    string tagName = TagsUtils.ExtractTagName(tag);
                    if (TagsUtils.IsCustomTag(tagName) && tagName != "")
                    {
                        TextCommand command = factory.CreateCommand(tagName);
                        if (tag.Contains("/"))
                        {
                            activeCommands.Remove(tagName);
                        }
                        else
                        {
                            if (!command.OneShot)
                            {
                                activeCommands.Add(tagName);
                                
                            }
                            else
                            {
                                result.commands[i- tagOffset].Add(command);
                            }
                        }
                        modifiedText = modifiedText.Replace(tag, "");
                    }
                    tag = "";
                    isInsideTag = false;
                }
            }
            else
            {
                foreach (string activeCommand in activeCommands)
                {
                    TextCommand command = factory.CreateCommand(activeCommand);
                    result.commands[i-tagOffset].Add(command);
                }
            }
        }
        result.processedText = modifiedText;
        return result;
    }

}
