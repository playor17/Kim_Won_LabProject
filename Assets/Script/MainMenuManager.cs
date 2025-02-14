using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel;
    public GameObject creditPanel;
    public GameObject optionsPanel;
    public GameObject tutorialPanel;
    public GameObject levelSelectPanel; // Panel untuk pemilihan level

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;

    void Start()
    {
        ShowMainMenu();
    }

    // Fungsi untuk tombol Play
    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    // Fungsi untuk memuat level spesifik
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void ShowCredit()
    {
        mainMenuPanel.SetActive(false);
        creditPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowTutorial()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(false);
        creditPanel.SetActive(false);
        optionsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}