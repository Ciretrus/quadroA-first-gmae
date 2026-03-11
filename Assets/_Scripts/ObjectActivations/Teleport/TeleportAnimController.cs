using System;
using UnityEngine;

public class TeleportAnimController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_pauseTimer = 1f;
    [SerializeField] private TeleportTrigger[] m_triggers;

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

    public void AnimateTeleportation(Action action)
    {
        StartCoroutine(AnimationController.AnimateTransitionWithPause(
            m_animator,
            action,
            ServiceLocator.Resolve<ItemsActivations>().ChangeMovementState,
            "FadeIn",
            "FadeOut",
            m_pauseTimer));
    }
}
