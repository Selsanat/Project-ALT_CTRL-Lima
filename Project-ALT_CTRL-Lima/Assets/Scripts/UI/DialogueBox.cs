using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;
    public string characterName { set { _nameText.text = value; } }

    [SerializeField]
    private TextMeshProUGUI _dialogueText;
    public string dialogue { set { _dialogueText.text = value; } }
}
