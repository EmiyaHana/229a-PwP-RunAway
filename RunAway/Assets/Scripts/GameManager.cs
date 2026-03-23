using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState { Playing, Paused, GameOver}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;
    public Transform spawnPoint;

    [Header("UI")]
    public GameObject hudPanel;
    public TextMeshProUGUI timerText;

    private float timer = 0f;
    
    void Awake() { Instance = this; }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f; 
        timer = 0f;
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
        }

        if (currentState == GameState.Playing)
        {
            timer += Time.deltaTime;
            if (timerText != null) 
            {
                timerText.text = "Time: " + timer.ToString("F2");
            }
        }
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 1f;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            Time.timeScale = 1f;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            currentState = GameState.GameOver;
            
            PlayerPrefs.SetFloat("FinalTime", timer);
            PlayerPrefs.Save();

            SceneManager.LoadScene("ResultMenu");
        }
    }
}