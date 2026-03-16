using UnityEngine;
using UnityEngine.InputSystem;
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

    private DiaryNotificationSystem m_notificationSystem;
    private Inventory m_inventory;
    private Tween m_tutorialTween;
    private Tween m_cursorTween; 
    private Tween m_inventoryTween;

    private void Start()
    {
        m_notificationSystem = ServiceLocator.Resolve<DiaryNotificationSystem>();
        m_inventory = ServiceLocator.Resolve<Inventory>();
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

    public void ShowPauseMenu(InputAction.CallbackContext context)
    {
        ServiceLocator.Resolve<PauseMenuView>().Pause();
    }
}
