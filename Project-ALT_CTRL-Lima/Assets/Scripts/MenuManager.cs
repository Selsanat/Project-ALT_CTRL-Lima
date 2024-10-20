using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Serializable]
    private struct InputMenu
    {
        public Button Button;
        public KeyCode Input;
    }

    [SerializeField]
    private List<InputMenu> InputMenus = new List<InputMenu>();

    private void Update()
    {
        foreach (InputMenu value in InputMenus)
        {
            if (Input.GetKeyDown(value.Input))
            {
                value.Button.onClick.Invoke();
            }
        }
    }
}
