using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private TMP_Text m_tutorialText;
    [SerializeField] private float m_tutorialFadeTime = 1f;
    [SerializeField] private float m_tutorialShowTime = 1f;
    [Header("Inventory")]
    [SerializeField] private GameObject[] m_inventoryItems;
    [SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private float m_inventoryFadeTime = 0.3f;
    [SerializeField] private float m_inventoryShowTime = 1f;
    [Header("Cursor")]
    [SerializeField] private Image m_objectActivationCursor;
    [SerializeField] private float m_cursorFadeTime = 0.3f;
    [SerializeField] private float m_cursorFadeValue = 0.5f;
    [Header("Notes")]
    [SerializeField] private Image m_noteImage;
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

    private DiaryNotificationSystem m_notificationSystem;
    private Inventory m_inventory;
    private Tween m_tutorialTween;
    private Tween m_cursorTween; 
    private Tween m_inventoryTween;
    private bool m_onPause = false;
    .
    private void Start()
    {
        m_notificationSystem = ServiceLocator.Resolve<DiaryNotificationSystem>();
        m_inventory = ServiceLocator.Resolve<Inventory>();
    }

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

    public void ShowObjectActivationCursor(bool shouldBeActivated)
    {
        float value = 0f;
        float time = m_cursorFadeTime;

        if (shouldBeActivated) 
        { 
            value = m_cursorFadeValue; 
            time *= 2; 
        }

        AnimationController.Fade(m_cursorTween, m_objectActivationCursor, value, time);
    }

    public void ShowDiaryNotification()
    {
        m_notificationSystem.TriggerNotification();
    }

    public void ShowNote(Sprite sprite)
    {
        m_noteImage.sprite = sprite;
        m_noteImage.gameObject.SetActive(!m_noteImage.gameObject.activeSelf);
    }

    public void ShowTutorial(string text)
    {
        m_tutorialText.text = text;

        AnimationController.FadeInAndOut(m_tutorialTween, m_tutorialText, m_tutorialFadeTime, m_tutorialShowTime);
    }

    public void ShowInventory(InputAction.CallbackContext context)
    {
        for (int i = 0; i < m_inventory.Counter; i++)
        {
            GameObject item = m_inventoryItems[i];
            var itemText = item.transform.GetComponentInChildren<TMP_Text>();
            Image itemImage = item.GetComponent<Image>();

            InventoryItem newItem = m_inventory.GetItem(i);

            if (newItem.count != 1)
            {
                Debug.Log(newItem.itemName);
                itemText.text = newItem.count.ToString();
            }
            else
            {
                itemText.text = "";
            }

            itemImage.sprite = newItem.image;

            AnimationController.FadeInAndOut(m_inventoryTween, itemImage, m_inventoryFadeTime, m_inventoryShowTime);
            AnimationController.FadeInAndOut(m_inventoryTween, itemText, m_inventoryFadeTime, m_inventoryShowTime);
        }
    }

    // TODO Rework everything down here
    public void Pause(InputAction.CallbackContext context)
    {
        Pause();
    }

    private void Pause()
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
