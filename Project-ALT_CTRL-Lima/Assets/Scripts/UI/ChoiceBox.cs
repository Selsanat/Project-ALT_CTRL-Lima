using TMPro;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _choiceA;
    public string choiceA {set {_choiceA.text = value;}}
    private int _redirectChoiceA;
    public int redirectChoiceA {get => _redirectChoiceA; set {_redirectChoiceA = value;}}

    [SerializeField]
    private TextMeshProUGUI _choiceB;
    public string choiceB {set {_choiceB.text = value;}}
    private int _redirectChoiceB;
    public int redirectChoiceB {get => _redirectChoiceB; set {_redirectChoiceB = value;}}
}
