using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum EyesState
{
    Default,
    Cyring,
    Hypnotized,
}

public class Character : MonoBehaviour
{
    private CharacterData _currentData;
    [SerializeField] private Image _body;
    [SerializeField] private Image _eyes;

    public CharacterData Data {get => _currentData;}

    public void SetEyesState(int state)
    {
        switch ((EyesState)state)
        {
            case EyesState.Default:
                _eyes.sprite = _currentData.DefaultEyes;
                break;
            case EyesState.Cyring:
                _eyes.sprite = _currentData.CryingEyes;
                break;
            case EyesState.Hypnotized:
                _eyes.sprite = _currentData.HypnotizedEyes;
                break;
        }
    }

    public void SetData(CharacterData Data)
    {
        _currentData = Data;
        _body.sprite = _currentData.Character;
        _eyes.sprite = _currentData.DefaultEyes;
    }
}
