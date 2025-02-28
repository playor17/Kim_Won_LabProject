using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI remainingEnemiesText;

    public GameObject gameOverPanel;
    public GameObject pausePanel;

    void Awake()
    {
        Instance = this;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score:N0}";
    }

    public void UpdateWeaponText(string weaponName)
    {
        if (weaponText != null)
            weaponText.text = $"Current Bubble: {weaponName}";
    }

    public void UpdateCombo(int multiplier)
    {
        if (comboText != null)
        {
            if (multiplier > 1)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = $"Combo x{multiplier}!";
            }
            else
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = $"X {health}";
    }

    public void UpdateRemainingEnemies(int remaining)
    {
        if (remainingEnemiesText != null)
            remainingEnemiesText.text = $"Remaining: {remaining} enemies";
    }

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            TextMeshProUGUI finalScoreText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
            if (finalScoreText != null)
            {
                finalScoreText.text = $"Final Score: {finalScore:N0}";
            }
        }
    }

    public void ShowPause(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);
        }
    }
}
