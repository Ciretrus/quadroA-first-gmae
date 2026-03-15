using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button m_resumeButton;
    [SerializeField] private Button m_newGameButton;
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private Button m_backToMenuButton;
    [SerializeField] private Button m_exitButton;
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_settings;

    private void OnEnable()
    {
        m_resumeButton.onClick.AddListener(ResumeGame);
        m_newGameButton.onClick.AddListener(StartNewGame);
        m_settingsButton.onClick.AddListener(ChangeSettingsState);
        m_backToMenuButton.onClick.AddListener(ChangeSettingsState);
        m_exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDisable()
    {
        m_resumeButton.onClick.RemoveListener(ResumeGame);
        m_newGameButton.onClick.RemoveListener(StartNewGame);
        m_settingsButton.onClick.RemoveListener(ChangeSettingsState);
        m_backToMenuButton.onClick.RemoveListener(ChangeSettingsState);
        m_exitButton.onClick.RemoveListener(ExitGame);
    }

    private void ResumeGame()
    {
        Debug.Log("Game resumed");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(GlobalConstants.Scenes.NewGameScene);
    }

    private void ChangeSettingsState()
    {
        m_settings.SetActive(!m_settings.activeSelf);
        m_mainMenu.SetActive(!m_mainMenu.activeSelf);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
