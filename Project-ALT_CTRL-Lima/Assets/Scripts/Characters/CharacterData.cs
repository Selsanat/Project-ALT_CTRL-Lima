using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Emotion
{
    None,
    Neutral,
    Passionate,
    Hypnotized,
    Sad,
    Exhausted,
    Livid,
    SurprisedButLivid,
    Calm,
    Impatient,
    Decontenance,
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
    [SerializeField] private TextAsset _dialogues;

    public Sprite Character { get => _character;}
    public List<EmotionStruct> Emotions {get => _emotions;}
    public TextAsset Dialogues {get => _dialogues;}
}
