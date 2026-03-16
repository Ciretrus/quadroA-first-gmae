using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiaryNotificationSystem : MonoBehaviour
{
    [SerializeField] private Image m_notifText;
    [SerializeField] private float m_fadeTime = 1.2f;
    [SerializeField] private float m_showTime = 1f;

    private Tween m_notifTween;

    public void TriggerNotification()
    {
        AnimationController.FadeInAndOut(m_notifTween, m_notifText, m_fadeTime, m_showTime);
    }
}