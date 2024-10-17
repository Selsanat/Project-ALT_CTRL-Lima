using UnityEngine;
using UnityEngine.UI;

public class PlayerBox : DialogBox
{
    [SerializeField] private Sprite _playerDialogueBox;
    [SerializeField] private Sprite _narratorDialogueBox;
    private Image _dialogueBoxImage;

    private void Start()
    {
        _dialogueBoxImage = GetComponent<Image>();
    }

    public void SetDialogBox(CharacterType type)
    {
#if UNITY_EDITOR
        if (type == CharacterType.Client)
        {
            Debug.LogWarning("Invalid type");
            return;
        }
#endif
        _dialogueBoxImage.sprite = type == CharacterType.Player ? _playerDialogueBox : _narratorDialogueBox;
    }
}
