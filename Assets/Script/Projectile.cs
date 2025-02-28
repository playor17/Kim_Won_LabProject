using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum ProjectileType { Rock, Paper, Scissors }
    public ProjectileType type;
    public float speed = 8f;

    void Update()
    {
        // Move to the right
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Destroy if exit screen
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
        if (enemy.type == Enemy.EnemyType.Boss)
        {
            HandleBossHit(enemy);
        }
        else
        {
            bool canDamage = false;
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

    void HandleBossHit(Enemy enemy)
    {
        switch (type)
        {
            case ProjectileType.Rock:
                enemy.hitByRock = true;
                break;
            case ProjectileType.Paper:
                enemy.hitByPaper = true;
                break;
            case ProjectileType.Scissors:
                enemy.hitByScissors = true;
                break;
        }

        // Hit all 3, destroy the boss
        if (enemy.hitByRock && enemy.hitByPaper && enemy.hitByScissors)
        {
            enemy.TakeDamage();
        }

        // Projectiles are always destroyed
        Destroy(gameObject);
    }

}