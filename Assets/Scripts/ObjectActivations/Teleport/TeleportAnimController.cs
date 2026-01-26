using System;
using UnityEngine;

public class TeleportAnimController : MonoBehaviour
{
    [SerializeField] private ItemsActivations m_itemsActivations;
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_pauseTimer = 1f;
    [SerializeField] private TeleportTrigger[] m_triggers;

    private void OnEnable()
    {
        foreach(var trigger in m_triggers)
        {
            trigger.OnTrigger += AnimateTeleportation;
        }
    }

    private void OnDisable()
    {
        foreach (var trigger in m_triggers)
        {
            trigger.OnTrigger -= AnimateTeleportation;
        }
    }

    public void AnimateTeleportation(Action action)
    {
        StartCoroutine(AnimationController.AnimateMethodWithPause(
            m_animator,
            action,
            m_itemsActivations.ChangeMovementState,
            "FadeIn",
            "FadeOut",
            m_pauseTimer));
    }
}
