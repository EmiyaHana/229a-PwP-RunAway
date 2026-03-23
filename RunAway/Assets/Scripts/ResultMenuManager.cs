using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultMenuManager : MonoBehaviour
{
    public TextMeshProUGUI timeDisplayText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float finalTime = PlayerPrefs.GetFloat("FinalTime", 0f);
        timeDisplayText.text = "You Escaped!\nTime: " + finalTime.ToString("F2") + " Seconds";
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}