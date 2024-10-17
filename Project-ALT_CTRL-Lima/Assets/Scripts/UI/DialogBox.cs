using TMPro;
using UnityEngine;

public abstract class DialogBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _dialogText;
    public string dialog { set { _dialogText.text = value; } }
}
