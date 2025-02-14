using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI comboText;
    public GameObject gameOverPanel;

    void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score:N0}";
    }

    public void UpdateWeaponText(string weaponName)
    {
        weaponText.text = $"Current Bubble: {weaponName}";
    }

    public void UpdateCombo(int multiplier)
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

    public void ShowGameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);
        TextMeshProUGUI finalScoreText = gameOverPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore:N0}";
        }
    }
}
