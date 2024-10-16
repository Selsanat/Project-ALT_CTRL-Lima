using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public abstract class TextCommand
{
    protected TMP_Text _text;
    protected TMP_TextInfo _textInfo;
    protected int _currentCharacter;
    protected int _meshIndex;
    protected int _vertexIndex;
    public virtual bool OnShot => false;
    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void SetupData(string strCommandData) { }

    protected void Init(TMP_Text text, TMP_TextInfo textInfo, int currentCharacter)
    {
        _text = text;
        _textInfo = textInfo;
        _currentCharacter = currentCharacter;
        _meshIndex = _textInfo.characterInfo[_currentCharacter].materialReferenceIndex;
        _vertexIndex = _textInfo.characterInfo[_currentCharacter].vertexIndex;
    }
    protected TMP_MeshInfo GetMeshInfo()
    {
        return _text.textInfo.meshInfo[_meshIndex];
    }
}