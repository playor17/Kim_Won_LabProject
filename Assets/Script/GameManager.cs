using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    [Header("Level System")]
    public int currentLevel = 1;
    public int enemiesDefeated = 0;
    public int enemiesNeededToComplete = 10; // Number of enemies needed to complete the level

    [Header("Scene Management")]
    public string[] levelScenes; // Array of scene names for each level

    [Header("Scoring")]
    public int score = 0;
    public int basePointsPerKill = 100;
    public float comboTimeWindow = 2f;
    private float lastKillTime;
    private int comboMultiplier = 1;

    [Header("Difficulty")]
    public float baseSpawnInterval = 2f;
    public float minimumSpawnRate = 0.5f;
    public float baseEnemySpeed = 3f;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI comboText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public GameObject pausePanel; // Tambahkan ini

    [Header("Progress UI")]
    public TextMeshProUGUI enemyProgressText;
    public Slider enemyProgressSlider;
    public TextMeshProUGUI remainingEnemiesText;

    [Header("Health System")]
    public int maxHealth = 5;
    public int currentHealth;
    public TextMeshProUGUI healthText;

    [Header("Game State")]
    public bool isGameOver = false;
    public bool isPaused = false;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenuScene";

    [Header("Audio")]
    public AudioSource backgroundMusicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip enemyDestroyedSFX;
    public AudioClip playerHitSFX;
    public AudioClip levelUpSFX;

    void Start()
    {
        InitializeGame();
        PlayBackgroundMusic();
    }

    void InitializeGame()
    {
        currentHealth = maxHealth;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Get current level from scene index
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        // Initialize progress slider if it exists
        if (enemyProgressSlider != null)
        {
            enemyProgressSlider.maxValue = enemiesNeededToComplete;
            enemyProgressSlider.value = 0;
        }

        enemiesDefeated = 0;
        UpdateAllUI();
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Time.time - lastKillTime > comboTimeWindow)
        {
            ResetCombo();
        }
        // Tambahkan pengecekan input ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void UpdateAllUI()
    {
        UpdateProgressUI();
        UpdateHealthUI();
        UpdateBaseUI();
    }

    void UpdateProgressUI()
    {
        if (enemyProgressText != null)
        {
            enemyProgressText.text = $"Progress: {enemiesDefeated} / {enemiesNeededToComplete}";
        }

        if (remainingEnemiesText != null)
        {
            int remaining = enemiesNeededToComplete - enemiesDefeated;
            remainingEnemiesText.text = $"Remaining: {remaining} enemies";
        }

        if (enemyProgressSlider != null)
        {
            enemyProgressSlider.value = enemiesDefeated;
        }
    }

    void UpdateBaseUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
        if (levelText != null)
            levelText.text = $"Level: {currentLevel}";
        if (comboText != null)
        {
            comboText.gameObject.SetActive(comboMultiplier > 1);
            if (comboMultiplier > 1)
            {
                comboText.text = $"Combo x{comboMultiplier}!";
            }
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"X {currentHealth}";
            Debug.Log($"Health UI updated: {healthText.text}");
        }
        else
        {
            Debug.LogWarning("Health Text reference is missing!");
        }
    }

    public void EnemyDefeated(Enemy.EnemyType defeatedType)
    {
        if (isGameOver) return;

        // Calculate points with combo
        int points = basePointsPerKill * comboMultiplier;

        // Add bonus points based on enemy type
        switch (defeatedType)
        {
            case Enemy.EnemyType.Scissors:
                points += 50;
                break;
            case Enemy.EnemyType.Rock:
                points += 30;
                break;
        }

        // Update score and combo
        score += points;
        UpdateCombo();

        // Update enemy count and check level progress
        enemiesDefeated++;
        CheckLevelProgress();

        // Update all UI elements
        UpdateAllUI();

        //sfx
        PlaySFX(enemyDestroyedSFX);
    }

    void CheckLevelProgress()
    {
        if (enemiesDefeated >= enemiesNeededToComplete && !isGameOver)
        {
            CompleteLevelAndAdvance();
        }
    }

    void CompleteLevelAndAdvance()
    {
        // Check if there's a next level scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Disable enemy spawner before loading next level
            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance.DisableSpawning();
            }

            // Load next level
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Game completed
            GameCompleted();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusicSource != null && backgroundMusic != null)
        {
            backgroundMusicSource.clip = backgroundMusic;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
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
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Congratulations!\nFinal Score: {score}";
            }
        }
    }

    void UpdateCombo()
    {
        if (Time.time - lastKillTime <= comboTimeWindow)
        {
            comboMultiplier++;
        }
        lastKillTime = Time.time;

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

    void ResetCombo()
    {
        comboMultiplier = 1;
        if (comboText != null)
        {
            comboText.gameObject.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        if (!isGameOver)
        {
            PlaySFX(playerHitSFX);
            currentHealth--;
            Debug.Log($"Health reduced to: {currentHealth}");
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        isGameOver = true;

        // Disable enemy spawner
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.DisableSpawning();
        }

        // Aktifkan panel game over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Update skor akhir
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
            PauseGame();
            pausePanel.SetActive(true);
        }
        else
        {
            ResumeGame();
            pausePanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        // Muat ulang scene yang sama
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");

        // Reset semua nilai game ke default
        ResetGameState();

        // Load scene menu utama
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void ResetGameState()
    {
        // Reset semua nilai ke default
        currentLevel = 1;
        score = 0;
        enemiesDefeated = 0;
        comboMultiplier = 1;
        currentHealth = maxHealth;
        isGameOver = false;
        isPaused = false;
        Time.timeScale = 1f;

        // Disable enemy spawner
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.DisableSpawning();
        }

        // Destroy all existing enemies
        Enemy[] existingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in existingEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        UpdateAllUI();
    }
}