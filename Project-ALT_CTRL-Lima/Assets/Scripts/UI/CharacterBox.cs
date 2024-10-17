using TMPro;
using UnityEngine;

public class CharacterBox : DialogBox
{
    [SerializeField]
    private TextMeshProUGUI _nameText;
    public string characterName { set { _nameText.text = value; } }
}
