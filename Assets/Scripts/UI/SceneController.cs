using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Button m_resumeButton;
    [SerializeField] private Button m_newGameButton;
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private GameObject m_settings;
    [SerializeField] private Button m_exitButton;
    [SerializeField] private Button m_backToMenuButton;

    private void Awake()
    {
        m_resumeButton.onClick.AddListener(ResumeGame);
        m_newGameButton.onClick.AddListener(StartNewGame);
        m_settingsButton.onClick.AddListener(OpenSettings);
        m_exitButton.onClick.AddListener(ExitGame);
        m_backToMenuButton.onClick.AddListener(BackToMenu);
    }

    private void ResumeGame()
    {
        Debug.Log("Game resumed");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(GlobalConstants.NewGameScene);
        Debug.Log("New game created");
    }

    private void OpenSettings()
    {
        SettingsLoop();
        Debug.Log("Opened settings");
    }

    private void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exited");
    }

    private void BackToMenu()
    {
        SettingsLoop();
        Debug.Log("Back to main menu");
    }

    // rename 
    private void SettingsLoop()
    {
        m_settings.SetActive(!m_settings.activeSelf);
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
