using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class DialogsController : MonoBehaviour
{
    [SerializeField] TMP_InputField dialogInput;
    [SerializeField] int charactersPerSecond = 50;
    [SerializeField] TMP_Text _dialogText;
    private float _readCharacterOffset = 0;
    private int _readMaxCharacters = 0;
    private TMP_TextInfo _textInfo;
    private bool isReadingText = false;

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
        _readCharacterOffset += charactersPerSecond * Time.deltaTime;
        _dialogText.maxVisibleCharacters = (int)_readCharacterOffset;
        if (_readCharacterOffset >= _readMaxCharacters) GoToEnd();
    }
    public void ReadText()
    {
        _dialogText.text = dialogInput.text;
        _dialogText.ForceMeshUpdate();
        _readCharacterOffset = 0;
        _readMaxCharacters = _dialogText.GetParsedText().Length;
        _dialogText.maxVisibleCharacters = 0;
        isReadingText = true;
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
                for (int j = 0; j < 4; ++j)
                {
                    _dialogText.textInfo.meshInfo[meshIndex].colors32[vertexIndex + j].a = 255;
                    //MakeLetterJump(meshIndex, vertexIndex + j);
                }

                _dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    private void MakeLetterJump(int meshIndex,int vertexIndex)
    {
        Vector3 vertex = _dialogText.textInfo.meshInfo[meshIndex].vertices[vertexIndex];
        DOTween.To(() => vertex, x => vertex = x, vertex + new Vector3(0, 5f, 0), 0.5f).OnUpdate(() =>
        {
            _dialogText.textInfo.meshInfo[meshIndex].vertices[vertexIndex] = vertex;
            _dialogText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        });
    }


    private static TextCommand[] _GenerateCommands(string text)
    {
        TextCommandsFactory factory = new TextCommandsFactory();
        List<TextCommand> commands = new List<TextCommand>();
        int startIndex = 0;
        bool isInsideTag = false;
        List<string> tags = new List<string>();
        for (int i = 0; i < text.Length; ++i)
        {
            char character = text[i];
            switch (character)
            {
                case '<':
                {
                    tags.Add("");
                    isInsideTag = true;
                    break;
                }
                case '>':
                {
                    if (isInsideTag)
                    {
                        isInsideTag = false;
                        i++;
                    }
                    break;
                }
            }
            if (isInsideTag)
            {
                if (character == '>') isInsideTag = false;
                tags[startIndex] += character;
            }
        }
        foreach (string tag in tags)
        {
            string tagName = TagsUtils.ExtractTagName(tag);
            if (TagsUtils.IsCustomTag(tagName) && tagName != "")
            {
                commands.Add(factory.CreateCommand("textshake"));
                print(factory.CreateCommand("textshake"));
            }
        }
        return commands.ToArray();
    }

}
