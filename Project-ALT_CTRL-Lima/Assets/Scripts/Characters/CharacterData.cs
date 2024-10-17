using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Emotion
{
    None,
    Neutral,
    Hypnotized,
    Passionate,
    Sad,
    Exhausted,
    Livid,
    SurprisedButLivid,
    Calm,
    Impatient,
    Decontenance,
    Happy,
    Outraged,
}

[Serializable]
public struct EmotionStruct
{
    public Emotion emotion;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "New CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private Sprite _character;
    [SerializeField] private List<EmotionStruct> _emotions;
    [SerializeField] private TextAsset _dialog;

    public Sprite Character { get => _character;}
    public List<EmotionStruct> Emotions {get => _emotions;}
    public TextAsset Dialog {get => _dialog;}
}
