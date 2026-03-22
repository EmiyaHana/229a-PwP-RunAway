using UnityEngine;
using TMPro;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject hudPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimeText;

    [Header("Game Elements")]
    public EnemyAI enemyAI;
    public Transform playerSpawnPoint;
    public GameObject player;

    private float timer = 0f;
    private bool enemySpawned = false;

    void Awake() { Instance = this; }

    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + timer.ToString("F2");

            if (timer >= 10f && !enemySpawned)
            {
                enemyAI.StartHunting();
                enemySpawned = true;
                Debug.Log("Enemy Spawned!");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
    }

    public void StartGame()
    {
        timer = 0f;
        enemySpawned = false;
        player.transform.position = playerSpawnPoint.position;
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        ChangeState(GameState.Playing);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void GameOver(bool isWin)
    {
        Time.timeScale = 0f;
        if (isWin)
        {
            finalTimeText.text = "You Escaped!\nTime: " + timer.ToString("F2") + "s";
            ChangeState(GameState.GameOver);
            winPanel.SetActive(true);
        }
        else
        {
            ChangeState(GameState.GameOver);
            losePanel.SetActive(true);
        }
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        mainMenuPanel.SetActive(newState == GameState.MainMenu);
        pausePanel.SetActive(newState == GameState.Paused);
        hudPanel.SetActive(newState == GameState.Playing);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        if (newState == GameState.MainMenu || newState == GameState.GameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (newState == GameState.Playing)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}