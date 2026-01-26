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

    // It will work only after adding Rigidbody to the Player
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("door hit something");
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("door hit player");
            m_animator.StopPlayback();
        }
    }
}
