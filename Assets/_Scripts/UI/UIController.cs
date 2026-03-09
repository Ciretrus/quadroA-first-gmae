using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_objectActivationText;
    [SerializeField] private DiaryNotificationSystem m_notificationSystem;
    [SerializeField] private Image m_noteImage;
    [SerializeField] private GameObject m_pauseMenuCanvas;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_settings;
    [Header("Pause menu buttons")]
    [SerializeField] private Button m_continueButton;
    [SerializeField] private Button m_settingsButton;
    [SerializeField] private Button m_backToPauseButton;
    [SerializeField] private Button m_backToMenuButton;
    [SerializeField] private Button m_exitButton;
    [Header("Inventory")]
    [SerializeField] private GameObject[] m_inventoryItems;
    [SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private float m_inventoryTimeFade = 0.3f;
    [SerializeField] private float m_inventoryShowTime = 1f;

    private List<DG.Tweening.Sequence> m_tweens = new List<DG.Tweening.Sequence>();
    private bool m_onPause = false;
    private Inventory m_inventory;

    private void Start()
    {
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

    public void ShowObjectActivationText(bool shouldBeActivated)
    {
        m_objectActivationText.SetActive(shouldBeActivated);
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

    public void ShowInventory(InputAction.CallbackContext context)
    {
        foreach (var tween in m_tweens)
        {
            tween?.Kill();
        }
        m_tweens.Clear();

        for (int i = 0; i < m_inventory.Counter; i++)
        {
            var item = m_inventoryItems[i];
            var itemText = item.transform.GetComponentInChildren<TextMeshProUGUI>();
            var itemImage = item.GetComponent<Image>();

            var newItem = m_inventory.GetItem(i);

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
            m_tweens.Add(sequenceImage);
            m_tweens.Add(sequenceText);

        }

    }
}
