using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _instance;
    public static EnemySpawner Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EnemySpawner>();
            }
            return _instance;
        }
    }

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public float[] laneYPositions = { -3f, -1f, 1f };
    public float spawnInterval = 2f;
    public float enemySpeed = 3f;

    private float nextSpawnTime;
    private bool isSpawningEnabled = true;
    private bool isLevelCompleted = false;

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

    void Start()
    {
        InitializeSpawner();
    }

    void InitializeSpawner()
    {
        if (GameManager.Instance != null)
        {
            spawnInterval = GameManager.Instance.baseSpawnInterval;
            enemySpeed = GameManager.Instance.baseEnemySpeed;

            Debug.Log($"[EnemySpawner] Initialized for Level {GameManager.Instance.currentLevel}");
        }

        nextSpawnTime = Time.time + spawnInterval;
        isSpawningEnabled = true;
        isLevelCompleted = false;
    }

    void Update()
    {
        if (ShouldSpawn() && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    bool ShouldSpawn()
    {
        if (!isSpawningEnabled || enemyPrefab == null) return false;
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return false;
        if (isLevelCompleted) return false;
        if (GameManager.Instance != null && GameManager.Instance.isPaused) return false;

        return true;
    }

    void SpawnEnemy()
    {
        if (GameManager.Instance == null) return;

        int randomLane = Random.Range(0, laneYPositions.Length);
        Vector3 spawnPosition = new Vector3(transform.position.x, laneYPositions[randomLane], 0f);

        GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        enemy.SetSpeed(enemySpeed);

        int currentLevel = GameManager.Instance.currentLevel;

        if (currentLevel == 4)  // Level 4 (Veteran)
        {
            if (Random.value < 0.2f)
            {
                enemy.SetEnemyType(Enemy.EnemyType.Boss);
                Debug.Log("[EnemySpawner] Spawned BOSS in Veteran Level");
            }
            else
            {
                enemy.SetEnemyType((Enemy.EnemyType)Random.Range(0, 3));
                Debug.Log("[EnemySpawner] Spawned Regular Enemy in Veteran Level");
            }
        }
        else
        {
            enemy.SetEnemyType((Enemy.EnemyType)Random.Range(0, 3));
            Debug.Log($"[EnemySpawner] Spawned Regular Enemy in Level {currentLevel}");
        }
    }


    public void UpdateSpawnSettings(float newInterval, float newSpeed)
    {
        spawnInterval = newInterval;
        enemySpeed = newSpeed;
        nextSpawnTime = Time.time + spawnInterval;
    }

    public void EnableSpawning()
    {
        isSpawningEnabled = true;
        isLevelCompleted = false;
        nextSpawnTime = Time.time + spawnInterval;
    }

    public void DisableSpawning()
    {
        isSpawningEnabled = false;
    }

    public void SetLevelCompleted()
    {
        isLevelCompleted = true;
        DisableSpawning();
    }

    public void ResetSpawner()
    {
        InitializeSpawner();
    }

    void OnEnable()
    {
        EnableSpawning();
    }

    void OnDisable()
    {
        DisableSpawning();
    }
}
