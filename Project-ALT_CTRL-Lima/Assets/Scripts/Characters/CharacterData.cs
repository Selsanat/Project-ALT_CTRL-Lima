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
    TakenAback,
    Happy,
    Outraged,
    Irritated,
    Haughty,
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
    [SerializeField] private Character _character;
    [SerializeField] private List<EmotionStruct> _emotions;
    [SerializeField] private TextAsset _dialog;
    [SerializeField] private float _characterTimerLenght = 50.0f;
    [SerializeField] private bool _foregroundLayer = false;

    public Character Character { get => _character; }
    public List<EmotionStruct> Emotions { get => _emotions; }
    public TextAsset Dialog { get => _dialog; }
    public float CharacterTimerLenght { get => _characterTimerLenght; }
    public bool ForegroundLayer { get => _foregroundLayer; }
}
