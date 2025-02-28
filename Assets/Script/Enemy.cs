using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Rock, Paper, Scissors, Boss }
    public EnemyType type;
    public float speed;
    private SpriteRenderer spriteRenderer;

    // Boss hit tracker
    public bool hitByRock = false;
    public bool hitByPaper = false;
    public bool hitByScissors = false;

    [Header("Sprites")]
    [SerializeField] private Sprite rockSprite;
    [SerializeField] private Sprite paperSprite;
    [SerializeField] private Sprite scissorsSprite;
    [SerializeField] private Sprite bossSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void Update()
    {
        // Move to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Destroy if exit screen
        if (transform.position.x < -10f)
            Destroy(gameObject);
    }

    void Start()
    {
        if (type == EnemyType.Rock || type == EnemyType.Paper || type == EnemyType.Scissors)
        {
            UpdateSprite();
        }
    }


    public void SetEnemyType(EnemyType newType)
    {
        type = newType;
        UpdateSprite();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void UpdateSprite()
    {
        // Set color by type
        switch (type)
        {
            case EnemyType.Rock:
                spriteRenderer.sprite = rockSprite;
                break;
            case EnemyType.Paper:
                spriteRenderer.sprite = paperSprite;
                break;
            case EnemyType.Scissors:
                spriteRenderer.sprite = scissorsSprite;
                break;
            case EnemyType.Boss:
                spriteRenderer.sprite = bossSprite;
                break;
        }
    }

    public void TakeDamage()
    {
        // added score system
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDefeated(type);
        }
        else
        {
            Debug.LogWarning("GameManager instance not found!");
        }
        //destroy enemy objects
        Destroy(gameObject);
    }
}