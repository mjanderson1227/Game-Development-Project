using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start button clicked");
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
}