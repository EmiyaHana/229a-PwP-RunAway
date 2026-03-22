using UnityEngine;
using TMPro;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    public GameObject enemyPrefab;
    public Transform spawnPoint;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }

        if (currentState == GameState.Playing)
        {
            timer += Time.deltaTime;

            if (timerText != null) 
            {
                timerText.text = "Time: " + timer.ToString("F2");
            }

            if (timer >= 10f && !enemySpawned)
            {
                if (enemyAI != null && playerSpawnPoint != null)
                {
                    enemyAI.transform.position = playerSpawnPoint.position; 
                    enemyAI.gameObject.SetActive(true); 
                    enemyAI.StartHunting(); 
                }
                enemySpawned = true;
                Debug.Log("Enemy Spawned and Hunting!");
            }
        }
    }

    public void StartGame()
    {
        GameManager.Instance.currentState = GameState.Playing;

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        Time.timeScale = 1f; 
        
        timer = 0f;
        enemySpawned = false;

        if (player != null)
        {
            PlayerController pController = player.GetComponent<PlayerController>();
            if (pController != null) pController.ResetPlayerState();

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = playerSpawnPoint.position;
            player.transform.rotation = playerSpawnPoint.rotation;

            if (cc != null) cc.enabled = true;
        }

        if (enemyAI != null)
        {
            enemyAI.gameObject.SetActive(false);
            enemyAI.transform.position = playerSpawnPoint.position;
        }

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

    public void ReturnToMainMenu()
    {
        if (enemyAI != null)
        {
            enemyAI.gameObject.SetActive(false);
            enemySpawned = false;
        }

        ChangeState(GameState.MainMenu);
    }

    public void RestartGame()
    {
        StartGame(); 
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        mainMenuPanel.SetActive(newState == GameState.MainMenu);
        pausePanel.SetActive(newState == GameState.Paused);
        hudPanel.SetActive(newState == GameState.Playing);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        if (newState == GameState.MainMenu || newState == GameState.GameOver || newState == GameState.Paused)
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