using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}