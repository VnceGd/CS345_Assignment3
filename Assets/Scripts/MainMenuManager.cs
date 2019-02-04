using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Load the game scene
    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // Quit the game
    void QuitGame()
    {
        Application.Quit();
    }
}
