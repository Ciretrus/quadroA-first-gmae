using UnityEngine;

public class Drawer : Usable
{
    [SerializeField] private Animator m_animator;
    private bool m_isOpened;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        if (m_isOpened)
        {
            m_animator.Play("MoveIn");
        }
        else
        {
            m_animator.Play("MoveOut");
        }

        m_isOpened = !m_isOpened;
    }
}
