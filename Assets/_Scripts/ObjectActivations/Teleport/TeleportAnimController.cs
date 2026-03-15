using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TeleportAnimController : MonoBehaviour
{
    [SerializeField] private TeleportTrigger[] m_triggers;
    [SerializeField] private Image m_blackImage;
    [SerializeField] private float m_fadeTime = 1f;
    [SerializeField] private float m_showTime = 1f;

    private Tween m_teleportTween;

    private void OnEnable()
    {
        foreach(var trigger in m_triggers)
        {
            trigger.Trigger += AnimateTeleportation;
        }
    }

    private void OnDisable()
    {
        foreach (var trigger in m_triggers)
        {
            trigger.Trigger -= AnimateTeleportation;
        }
    }

    public void AnimateTeleportation(Action teleport)
    {
        StartCoroutine(
            AnimationController.FadeInAndOut(
                teleport, 
                ServiceLocator.Resolve<ItemsActivations>().ChangeMovementState, 
                m_teleportTween, 
                m_blackImage, 
                m_fadeTime, 
                m_showTime)
            );
    }
}
