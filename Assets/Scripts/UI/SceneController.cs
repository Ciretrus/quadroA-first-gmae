using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Button m_resumeButton;
    [SerializeField] private Button m_newGameButton;
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private Button m_backToMenuButton;
    [SerializeField] private Button m_exitButton;
    [SerializeField] private GameObject m_settings;

    private void Awake()
    {
        m_resumeButton.onClick.AddListener(ResumeGame);
        m_newGameButton.onClick.AddListener(StartNewGame);
        m_settingsButton.onClick.AddListener(ChangeSettingsState);
        m_backToMenuButton.onClick.AddListener(ChangeSettingsState);
        m_exitButton.onClick.AddListener(ExitGame);
    }

    private void ResumeGame()
    {
        // TODO Create save system
        Debug.Log("Game resumed");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(GlobalConstants.NewGameScene);
    }

    private void ChangeSettingsState()
    {
        m_settings.SetActive(!m_settings.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
