using UnityEngine;

public class DiaryNotificationSystem : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_pauseTimer = 1f;

    public void TriggerNotification()
    {
        StartCoroutine(AnimationController.AnimateWithPause(
            m_animator, 
            "NotificationFadeIn", 
            "NotificationFadeOut", 
            m_pauseTimer));
    }
}