using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private int currentLane = 1;
    private float[] laneYPositions = { -3f, -1f, 1f };

    [Header("Shooting")]
    public GameObject[] projectilePrefabs; // 0:Rock, 1:Paper, 2:Scissors
    public float shootCooldown = 0.5f;
    private float lastShootTime;
    private int currentProjectileType = 0;

    [Header("UI References")]
    public TextMeshProUGUI weaponText;

    void Update()
    {
        // Pindah lane
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentLane < 2)
        {
            currentLane++;
            UpdateLanePosition();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentLane > 0)
        {
            currentLane--;
            UpdateLanePosition();
        }

        // Shooting
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }

        // Switch projectile type directly by key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentProjectileType = 0;   // weapon 1
            UpdateProjectileIndicator();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            currentProjectileType = 1;   // weapon 2
            UpdateProjectileIndicator();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            currentProjectileType = 2;   // weapon 3
            UpdateProjectileIndicator();
        }
    }

    void UpdateLanePosition()
    {
        Vector3 pos = transform.position;
        pos.y = laneYPositions[currentLane];
        transform.position = pos;
    }

    void Shoot()
    {
        Vector3 spawnPos = transform.position + Vector3.right;
        Instantiate(projectilePrefabs[currentProjectileType], spawnPos, Quaternion.identity);
    }

    void UpdateProjectileIndicator()
    {
        // Will be implemented with UI later
        Debug.Log($"Current weapon: {(Projectile.ProjectileType)currentProjectileType}");
        if (weaponText != null)
        {
            string weaponName = currentProjectileType switch
            {
                0 => "Blue Bubble",
                1 => "Green Bubble",
                2 => "Red Bubble",
                _ => "Unknown"
            };
            weaponText.text = $"Current Weapon: {weaponName}";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Reduce player's life
            if (GameManager.Instance != null)
            {
                //debug health
                Debug.Log("Player hit by enemy!");
                GameManager.Instance.TakeDamage();
                // Destroy the enemy that crashes
                Destroy(other.gameObject);
            }
        }
    }
}