using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public void ResumeButton()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}