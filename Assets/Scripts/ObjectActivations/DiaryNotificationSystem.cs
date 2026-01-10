using System.Collections;
using UnityEngine;

public class DiaryNotificationSystem : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_disableTimer = 1f;

    public void TriggerNotification()
    {
        StartCoroutine(EnableNotification());
    }

    private IEnumerator EnableNotification()
    {
        m_animator.Play("NotificationFadeIn");
        yield return new WaitForSeconds(m_disableTimer);
        m_animator.Play("NotificationFadeOut");
    }
}