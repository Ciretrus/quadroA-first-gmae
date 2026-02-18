using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent (typeof(Collider))]
public class Door : Usable
{
    [SerializeField] private Vector3 m_angleRotation = new Vector3(0, -90, 0);
    [SerializeField] private Collider m_collider;
    [SerializeField] private float m_timer = 0.5f;

    private Tweener m_tween;
    private bool m_isOpened;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        m_collider.enabled = false; 
        m_tween?.Kill();

        if (m_isOpened)
        {
            m_tween = transform.DORotate(Vector3.zero, m_timer);
        }
        else
        {
            m_tween = transform.DORotate(m_angleRotation, m_timer);
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
