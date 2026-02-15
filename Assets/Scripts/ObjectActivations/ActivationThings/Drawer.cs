using UnityEngine;
using DG.Tweening;

public class Drawer : Usable
{
    [SerializeField] private Vector3 m_newPosition = new Vector3(0, 0, -0.7f);
    [SerializeField] private float m_timer = 1f;

    private Vector3 m_initialPosition;
    private Tweener m_tween;
    private bool m_isOpened;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
        m_initialPosition = transform.localPosition;
    }

    public override void Use()
    {
        m_tween?.Kill();

        if (m_isOpened)
        {
            m_tween = transform.DOLocalMove(m_initialPosition, m_timer);
        }
        else
        {
            m_tween = transform.DOLocalMove(m_initialPosition - m_newPosition, m_timer);
        }

        m_isOpened = !m_isOpened;
    }
}
