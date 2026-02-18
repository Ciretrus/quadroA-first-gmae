using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject m_objectActivationText;
    [SerializeField] private DiaryNotificationSystem m_notificationSystem;

    public void ShowObjectActivationText(bool shouldBeActivated)
    {
        m_objectActivationText.SetActive(shouldBeActivated);
    }

    public void ShowDiaryNotification()
    {
        m_notificationSystem.TriggerNotification();
    }
}
