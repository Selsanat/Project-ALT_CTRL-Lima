using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

public class DialogsController : MonoBehaviour
{
    [SerializeField] TMP_InputField dialogInput;
    [SerializeField] private int BASE_CHAR_PER_SEC = 15;
    public int charactersPerSecond = 50;
    [SerializeField] TMP_Text _dialogText;
    private float _readCharacterOffset = 0;
    private int _readMaxCharacters = 0;
    private TMP_TextInfo _textInfo;
    public bool isReadingText = false;
    private ProcessedText _currentProcessedText;
    public static DialogsController instance;
    public static DialogsController subTextboxInstance1;
    public static DialogsController subTextboxInstance2;
    private float pauseTime = 0;
    private float time = 0;
    [SerializeField] private float characterSoundDelay = 0.15f;

    public bool bIsReadingText { get => isReadingText; }

    private void Awake()
    {
        instance = this;
        time = Time.time;
    }

    public void SetCharacterPerSeconds(int value)
    {
        charactersPerSecond = value;
    }
    public void SetPauseTime(float value)
    {
        pauseTime += value;
    }
    private class ProcessedText
    {
        public int offset = 0;
        public string processedText = "";
        public Dictionary<int, List<TextCommand>> commands = new Dictionary<int, List<TextCommand>>();
    }
    private void Update()
    {
        if (isReadingText)
        {
            
            if (Time.time- time> characterSoundDelay)
            {
                time = Time.time;
                StartCoroutine(PlayTextSound());
            }
        }
        _UpdateReadText();
    }
    private IEnumerator PlayTextSound()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, characterSoundDelay/3));
        SoundManager.instance.PlayClip("Boop");
    }

    public void SkipAnimation()
    {
        _dialogText.ForceMeshUpdate();
        GoToEnd();
    }

    public void GoToEnd()
    {
        isReadingText = false;
    }
    private void _UpdateReadText()
    {
        if (!isReadingText) return;
        if (pauseTime > 0)
        {
            pauseTime -= Time.deltaTime;
            return;
        }
        int nextLetterCalcul = (int)(_readCharacterOffset + charactersPerSecond * Time.deltaTime);
        bool hasSecondPassed = (int)_readCharacterOffset + 1 == nextLetterCalcul; // Check if a second has passed
        _readCharacterOffset += charactersPerSecond * Time.deltaTime;
        if (hasSecondPassed && _readCharacterOffset < _readMaxCharacters )
        {
            ShowCharacter((int)_readCharacterOffset - 1);
            _currentProcessedText.commands[nextLetterCalcul - 1].ForEach(command => {
                command.OnEnter();
            });
        }
        if (_readCharacterOffset >= _readMaxCharacters) GoToEnd();
    }

    public void ReadText(string text)
    {
        _dialogText.text = "";
        if (_currentProcessedText != null)
        {
            foreach (KeyValuePair<int, List<TextCommand>> command in _currentProcessedText.commands)
            {
                command.Value.ForEach(c =>
                {
                    c.OnExit();
                });
            }
        }
        charactersPerSecond = BASE_CHAR_PER_SEC;
        pauseTime = 0;
        _currentProcessedText = _GenerateCommands(text);
        _dialogText.text = _currentProcessedText.processedText;
        _textInfo = _dialogText.textInfo;
        _readCharacterOffset = 0;
        _readMaxCharacters = _currentProcessedText.processedText.Length - _currentProcessedText.offset;
        _dialogText.ForceMeshUpdate();
        initText();
        HideText();
        isReadingText = true;
    }

    public void ShowCharacter(int characterIndex)
    {
        int meshIndex = _dialogText.textInfo.characterInfo[characterIndex].materialReferenceIndex;
        int vertexIndex = _dialogText.textInfo.characterInfo[characterIndex].vertexIndex;
        if (!char.IsWhiteSpace(_dialogText.textInfo.characterInfo[characterIndex].character))
        {   
            for (int j = 0; j < 4; ++j)
            {
                _dialogText.textInfo.meshInfo[meshIndex].colors32[vertexIndex + j].a = 255;
            }
            _dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }
    }
    public void HideText()
    {
        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            int meshIndex = _dialogText.textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = _dialogText.textInfo.characterInfo[i].vertexIndex;
            if (!char.IsWhiteSpace(_dialogText.textInfo.characterInfo[i].character))
            {
                for (int j = 0; j < 4; ++j)
                {
                    _dialogText.textInfo.meshInfo[meshIndex].colors32[vertexIndex + j].a = 0;
                }
                _dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            }
        }
    }
    public void initText()
    {
        foreach (KeyValuePair<int, List<TextCommand>> command in _currentProcessedText.commands)
        {
            command.Value.ForEach(c =>
            {
                //print(command.Key);
                    c.Init(_dialogText, command.Key);
            });
        }
    }

    //Function for test purposes
    public void showDialog()
    {
        ReadText(dialogInput.text);
    }

    public void playDialog(TMP_Text textBox, string text)
    {
        _dialogText = textBox;

#if UNITY_EDITOR
        if (_dialogText.isTextOverflowing)
        {
            Debug.LogWarning(text + " is overflowing in " + textBox.name);
        }
#endif

        ReadText(text);
    }

    private static ProcessedText _GenerateCommands(string text)
    {
        string modifiedText = text;
        ProcessedText result = new ProcessedText();
        TextCommandsFactory factory = new TextCommandsFactory();
        List<(string, string)> activeCommands = new List<(string,string)>();
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
                        command.SetupData(TagsUtils.ExtractTagArgs(tag));
                        if (tag.Contains("/"))  
                        {
                            activeCommands.RemoveAt(activeCommands.FindLastIndex(x => x.Item1 == tagName));
                        }
                        else
                        {
                            if (!command.isOneShot)
                            {
                                activeCommands.Add((tagName, TagsUtils.ExtractTagArgs(tag)));
                                
                            }
                            else
                            {
                                result.commands[i- tagOffset+1].Add(command);
                            }
                        }
                        modifiedText = modifiedText.Replace(tag, string.Empty);
                    }
                    else
                    {
                        result.offset += tag.Length;
                    }
                    tag = "";
                    isInsideTag = false;
                }
            }
            else
            {
                foreach ((string, string) activeCommand in activeCommands)
                {
                    TextCommand command = factory.CreateCommand(activeCommand.Item1);
                    command.SetupData(activeCommand.Item2);
                    result.commands[i - tagOffset].Add(command);
                }
            }
        }
        result.processedText = modifiedText;
        return result;
    }

}
