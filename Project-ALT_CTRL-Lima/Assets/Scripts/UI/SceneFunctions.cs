using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFunctions : MonoBehaviour
{
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
