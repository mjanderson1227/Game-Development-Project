using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textObject;
    public void Setup(string text)
    {
        gameObject.SetActive(true);
        textObject.text = text;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }
}
