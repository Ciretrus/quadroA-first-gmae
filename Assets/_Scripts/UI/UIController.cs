using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private TMP_Text m_tutorialText;
    [SerializeField] private float m_tutorialTimeFade = 1f;
    [SerializeField] private float m_tutorialShowTime = 1f;
    [Header("Inventory")]
    [SerializeField] private GameObject[] m_inventoryItems;
    [SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private float m_inventoryTimeFade = 0.3f;
    [SerializeField] private float m_inventoryShowTime = 1f;
    [Header("Cursor")]
    [SerializeField] private Image m_objectActivationCursor;
    [SerializeField] private float m_cursorTimeFade = 0.3f;
    [SerializeField] private float m_cursorFade = 0.5f;
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
    private List<Sequence> m_inventoryTweens = new ();
    private Sequence m_tutorialTween;
    private Tweener m_cursorTween;
    private bool m_onPause = false;

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
        float fade = 0f;
        float time = m_cursorTimeFade;

        if (shouldBeActivated) 
        { 
            fade = m_cursorFade; 
            time = m_cursorTimeFade * 2; 
        }
        m_cursorTween?.Kill();
        m_cursorTween = m_objectActivationCursor.DOFade(fade, time);
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
        m_tutorialTween?.Kill();

        m_tutorialText.text = text;

        var sequence = DOTween.Sequence();

        sequence.Append(m_tutorialText.DOFade(1f, m_tutorialTimeFade));
        sequence.AppendInterval(m_tutorialShowTime);
        sequence.Append(m_tutorialText.DOFade(0f, m_tutorialTimeFade));

        m_tutorialTween = sequence;
    }

    public void ShowInventory(InputAction.CallbackContext context)
    {
        foreach (var tween in m_inventoryTweens)
        {
            tween?.Kill();
        }
        m_inventoryTweens.Clear();

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

            else itemText.text = "";

            itemImage.sprite = newItem.image;

            var sequenceImage = DOTween.Sequence();
            var sequenceText = DOTween.Sequence();

            sequenceImage.Append(itemImage.DOFade(1f, m_inventoryTimeFade));
            sequenceImage.AppendInterval(m_inventoryShowTime);
            sequenceImage.Append(itemImage.DOFade(0f, m_inventoryTimeFade));

            sequenceText.Append(itemText.DOFade(1f, m_inventoryTimeFade));
            sequenceText.AppendInterval(m_inventoryShowTime);
            sequenceText.Append(itemText.DOFade(0f, m_inventoryTimeFade));

            Debug.Log("TRY DOTWEEN IMAGE");
            m_inventoryTweens.Add(sequenceImage);
            m_inventoryTweens.Add(sequenceText);
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
