using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Serializable]
    private struct InputButton
    {
        public Button Button;
        public KeyCode Input;
    }

    [SerializeField]
    private List<InputButton> InputButtons = new List<InputButton>();

    private void Update()
    {
        foreach (InputButton value in InputButtons)
        {
            if (Input.GetKeyDown(value.Input))
            {
                value.Button.onClick.Invoke();
            }
        }
    }

    public static void OpenLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        print("Quit");
#endif
        Application.Quit();
    }
}
