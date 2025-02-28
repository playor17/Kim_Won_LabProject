using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int SelectedLevel = 1;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    [Header("Level System")]
    public int currentLevel = 1;
    public int enemiesDefeated = 0;
    public int enemiesNeededToComplete = 10;

    [Header("Scoring")]
    public int score = 0;
    public int basePointsPerKill = 100;
    public float comboTimeWindow = 2f;
    private float lastKillTime = 0f;
    private int comboMultiplier = 1;

    [Header("Difficulty")]
    public float baseSpawnInterval = 2f;
    public float minimumSpawnRate = 0.5f;
    public float baseEnemySpeed = 3f;

    [Header("Health System")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI remainingEnemiesText;
    public GameObject gameOverPanel;
    public GameObject pausePanel;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenuScene";

    [Header("Audio")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;
    public AudioClip backgroundMusic;
    public AudioClip enemyDestroyedSFX;
    public AudioClip playerHitSFX;
    public AudioClip levelUpSFX;

    public bool isGameOver = false;
    public bool isPaused = false;

    void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
    }

    void Start()
    {
        SelectedLevel = PlayerPrefs.GetInt("SelectedLevel", 1);
        currentLevel = SelectedLevel;

        InitializeGame();
        PlayBackgroundMusic();

        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.ResetSpawner();
        }

        UpdateAllUI();
    }

    void InitializeGame()
    {
        currentHealth = maxHealth;
        enemiesDefeated = 0;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        UpdateAllUI();
    }

    void Update()
    {
        if (Time.time - lastKillTime > comboTimeWindow)
        {
            ResetCombo();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void UpdateAllUI()
    {
        UpdateScoreUI();
        UpdateComboUI();
        UpdateHealthUI();
        UpdateRemainingEnemiesUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    void UpdateComboUI()
    {
        if (comboText != null)
        {
            if (comboMultiplier > 1)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = $"Combo x{comboMultiplier}!";
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = $"X {currentHealth}";
    }

    void UpdateRemainingEnemiesUI()
    {
        if (remainingEnemiesText != null)
            remainingEnemiesText.text = $"Remaining: {enemiesNeededToComplete - enemiesDefeated} enemies";
    }

    public void EnemyDefeated(Enemy.EnemyType type)
    {
        if (isGameOver) return;

        int points = basePointsPerKill * comboMultiplier;

        if (type == Enemy.EnemyType.Scissors) points += 50;
        if (type == Enemy.EnemyType.Rock) points += 30;

        score += points;
        UpdateCombo();

        enemiesDefeated++;
        UpdateAllUI();

        CheckLevelProgress();
        PlaySFX(enemyDestroyedSFX);
    }

    void CheckLevelProgress()
    {
        if (enemiesDefeated >= enemiesNeededToComplete)
        {
            CompleteLevelAndAdvance();
        }
    }

    void CompleteLevelAndAdvance()
    {
        currentLevel++;
        if (currentLevel > 4)
        {
            GameCompleted();
        }
        else
        {
            SceneManager.LoadScene(GetSceneNameForLevel(currentLevel));
        }
    }

    string GetSceneNameForLevel(int level)
    {
        return level switch
        {
            1 => "Level1",
            2 => "Level2",
            3 => "Level3",
            4 => "Veteran",
            _ => "Level1"
        };
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource && backgroundMusic)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource && clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    void GameCompleted()
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            var finalScoreText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Final Score: {score}";
            }
        }
    }

    void UpdateCombo()
    {
        if (Time.time - lastKillTime <= comboTimeWindow)
        {
            comboMultiplier++;
        }
        else
        {
            comboMultiplier = 1;
        }
        lastKillTime = Time.time;

        UpdateComboUI();
    }

    void ResetCombo()
    {
        comboMultiplier = 1;
        UpdateComboUI();
    }

    public void TakeDamage()
    {
        if (isGameOver) return;

        PlaySFX(playerHitSFX);
        currentHealth--;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        EnemySpawner.Instance.DisableSpawning();
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            var finalScoreText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Final Score: {score}";
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
