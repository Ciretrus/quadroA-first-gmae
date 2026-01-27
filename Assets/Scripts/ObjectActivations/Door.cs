using UnityEngine;

public class Door : Usable
{
    private Animator m_animator;
    private bool m_isOpened;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
        m_animator = GetComponent<Animator>();
    }

    public override void Use()
    {
        if (m_isOpened)
        {
            transform.Rotate(Vector3.up, 90);
            m_animator.Play("Close");
        }
        else
        {
            transform.Rotate(Vector3.up, -90);
            m_animator.Play("Open");
        }

        m_isOpened = !m_isOpened;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            m_animator.StopPlayback();
        }
    }
}
