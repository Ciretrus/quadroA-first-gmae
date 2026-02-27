using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_objectActivationText;
    [SerializeField] private DiaryNotificationSystem m_notificationSystem;
    [SerializeField] private GameObject[] m_inventoryItems;
    [SerializeField] private GameObject m_inventoryUI;
    [SerializeField] private float m_inventoryTimeFade = 0.3f;
    [SerializeField] private float m_inventoryShowTime = 1f;

    private List<DG.Tweening.Sequence> m_tweens = new List<DG.Tweening.Sequence>();
    public void ShowObjectActivationText(bool shouldBeActivated)
    {
        m_objectActivationText.SetActive(shouldBeActivated);
    }

    public void ShowDiaryNotification()
    {
        m_notificationSystem.TriggerNotification();
    }
    public void ShowInventory(InputAction.CallbackContext context)
    {
        foreach (var tween in m_tweens)
        {
            tween?.Kill();           
        }
        m_tweens.Clear();

        for (int i = 0; i < Inventory.instance.Counter; i++)
        {
            var item = m_inventoryItems[i];
            var itemText = item.transform.GetComponentInChildren<TextMeshProUGUI>();
            var itemImage = item.GetComponent<Image>();
            
            var newItem = Inventory.instance.GetItem(i);

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
