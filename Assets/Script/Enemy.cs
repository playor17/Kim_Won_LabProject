using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Rock, Paper, Scissors }
    public EnemyType type;
    public float speed;
    private SpriteRenderer spriteRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite rockSprite;
    [SerializeField] private Sprite paperSprite;
    [SerializeField] private Sprite scissorsSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void Update()
    {
        // Gerak ke kiri
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Destroy jika keluar screen
        if (transform.position.x < -10f)
            Destroy(gameObject);
    }

    void Start()
    {
        // Set random type
        type = (EnemyType)Random.Range(0, 3);
        UpdateSprite();
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
        // Set warna berdasarkan type
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
        }
    }

    public void TakeDamage()
    {
        // ditambahkan score system
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDefeated(type);
        }
        else
        {
            Debug.LogWarning("GameManager instance not found!");
        }
        //hancurkan enemy obyek
        Destroy(gameObject);
    }
}