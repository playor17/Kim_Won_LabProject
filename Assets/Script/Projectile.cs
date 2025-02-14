using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum ProjectileType { Rock, Paper, Scissors }
    public ProjectileType type;
    public float speed = 8f;

    void Update()
    {
        // Bergerak ke kanan
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Destroy jika keluar screen
        if (transform.position.x > 15f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                CheckDamage(enemy);
            }
        }
    }

    void CheckDamage(Enemy enemy)
    {
        bool canDamage = false;

        // Rock Paper Scissors Logic
        switch (type)
        {
            case ProjectileType.Rock:
                canDamage = enemy.type == Enemy.EnemyType.Scissors;
                break;
            case ProjectileType.Paper:
                canDamage = enemy.type == Enemy.EnemyType.Rock;
                break;
            case ProjectileType.Scissors:
                canDamage = enemy.type == Enemy.EnemyType.Paper;
                break;
        }

        if (canDamage)
        {
            enemy.TakeDamage();
            Destroy(gameObject);
        }
    }
}