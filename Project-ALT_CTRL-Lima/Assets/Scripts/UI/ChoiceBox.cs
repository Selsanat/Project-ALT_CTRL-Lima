using TMPro;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _choiceA;
    public string choiceA {set {_choiceA.text = value;}}

    public int redirectChoiceA;

    [SerializeField]
    private TextMeshProUGUI _choiceB;
    public string choiceB {set {_choiceB.text = value;}}

    public int redirectChoiceB;
}
