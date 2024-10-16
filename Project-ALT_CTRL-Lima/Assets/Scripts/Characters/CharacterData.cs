using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private Sprite _character;
    [SerializeField] private Sprite _defaultEyes;
    [SerializeField] private Sprite _cryingEyes;
    [SerializeField] private Sprite _hypnotizedEyes;
    [SerializeField] private TextAsset _dialogues;

    public Sprite Character { get => _character;}
    public Sprite DefaultEyes {get => _defaultEyes;}
    public Sprite CryingEyes {get => _cryingEyes;}
    public Sprite HypnotizedEyes {get => _hypnotizedEyes;}
    public TextAsset Dialogues {get => _dialogues;}
}
