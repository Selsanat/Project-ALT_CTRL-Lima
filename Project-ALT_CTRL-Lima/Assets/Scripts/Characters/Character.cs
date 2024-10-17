using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private CharacterData _currentData;
    [SerializeField] private Image _body;
    [SerializeField] private Image _eyes;

    private Dictionary<Emotion, Sprite> _emotionData = new Dictionary<Emotion, Sprite>();

    public CharacterData Data {get => _currentData;}

    public void SetEmotion(Emotion emotion)
    {
        if(_emotionData.TryGetValue(emotion, out Sprite sprite))
        {
            _eyes.sprite = sprite;
        }

#if UNITY_EDITOR
        else
        {
            Debug.LogWarning("This character does not have the emotion: " + emotion.ToString());
        }
#endif
    }

    public void SetData(CharacterData Data)
    {
        _currentData = Data;
        _body.sprite = _currentData.Character;

        foreach (EmotionStruct data in _currentData.Emotions)
        {
            _emotionData.Add(data.emotion, data.sprite);
        }
    }
}
