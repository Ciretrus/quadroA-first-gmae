using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class Door : Usable
{
    [SerializeField] private Collider m_collider;
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_timer = 1f;
    private bool m_isOpened;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        m_collider.enabled = false;

        if (m_isOpened)
        {
            m_animator.Play("Close");
        }
        else
        {
            m_animator.Play("Open");
        }

        StartCoroutine(EnableCollision());
        m_isOpened = !m_isOpened;
    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(m_timer);
        m_collider.enabled = true;
    }
}
