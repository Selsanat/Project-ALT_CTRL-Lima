using TMPro;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    public TextMeshProUGUI _choiceA;
    private int _redirectChoiceA;
    public int redirectChoiceA {get => _redirectChoiceA; set {_redirectChoiceA = value;}}

    public TextMeshProUGUI _choiceB;
    private int _redirectChoiceB;
    public int redirectChoiceB {get => _redirectChoiceB; set {_redirectChoiceB = value;}}
}
