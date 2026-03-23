using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenHowToPlay()
    {
        SceneManager.LoadScene("HowToPlayScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}