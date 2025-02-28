using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel;
    public GameObject creditPanel;
    public GameObject optionsPanel;
    public GameObject tutorialPanel;
    public GameObject levelSelectPanel;

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;

    void Start()
    {
        ShowMainMenu();
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void StartLevel1() { StartLevel(1, "Level1"); }
    public void StartLevel2() { StartLevel(2, "Level2"); }
    public void StartLevel3() { StartLevel(3, "Level3"); }
    public void StartVeteran() { StartLevel(4, "Veteran"); }

    void StartLevel(int level, string sceneName)
    {
        PlayerPrefs.SetInt("SelectedLevel", level);  // PlayerPrefs에 저장
        PlayerPrefs.Save();                          // 저장 즉시 반영
        Debug.Log($"[MainMenuManager] Trying to load {sceneName} for Level {level}");
        SceneManager.LoadScene(sceneName);           // 씬 로드
    }


    public void ShowCredit() { SwitchPanel(creditPanel); }
    public void ShowOptions() { SwitchPanel(optionsPanel); }
    public void ShowTutorial() { SwitchPanel(tutorialPanel); }
    public void ShowMainMenu() { SwitchPanel(mainMenuPanel); }

    void SwitchPanel(GameObject panel)
    {
        mainMenuPanel.SetActive(false);
        creditPanel.SetActive(false);
        optionsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        panel.SetActive(true);
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
