using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    [Header("Pause menu elements")]
    [SerializeField] private GameObject m_pauseMenuCanvas;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_settings;

    [Header("Pause menu buttons")]
    [SerializeField] private Button m_continueButton;
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private Button m_backToPauseButton;
    [SerializeField] private Button m_backToMenuButton;
    [SerializeField] private Button m_exitButton;
    
    private bool m_onPause = false;

    private void OnEnable()
    {
        m_continueButton.onClick.AddListener(Pause);
        m_settingsButton.onClick.AddListener(ChangeSettingsState);
        m_backToPauseButton.onClick.AddListener(ChangeSettingsState);
        m_backToMenuButton.onClick.AddListener(GoToMenu);
        m_exitButton.onClick.AddListener(ExitGame);
    }

    private void OnDisable()
    {
        m_continueButton.onClick.RemoveListener(Pause);
        m_settingsButton.onClick.RemoveListener(ChangeSettingsState);
        m_backToPauseButton.onClick.RemoveListener(ChangeSettingsState);
        m_backToMenuButton.onClick.RemoveListener(GoToMenu);
        m_exitButton.onClick.RemoveListener(ExitGame);
    }

    public void Pause()
    {
        if (m_onPause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            m_pauseMenu.SetActive(false);
            m_settings.SetActive(false);
            m_pauseMenuCanvas.SetActive(false);

            Time.timeScale = 1f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            m_pauseMenu.SetActive(true);
            m_pauseMenuCanvas.SetActive(true);

            Time.timeScale = 0f;
        }

        m_onPause = !m_onPause;
    }

    private void ChangeSettingsState()
    {
        m_settings.SetActive(!m_settings.activeSelf);
        m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(GlobalConstants.Scenes.MainMenuScene);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
