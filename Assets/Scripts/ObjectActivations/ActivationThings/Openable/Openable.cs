using UnityEngine;
using DG.Tweening;

public class Openable : Usable
{
    [SerializeField] private Vector3 m_newPosition = new Vector3(0, 0, -0.7f);
    [SerializeField] private Vector3 m_newRotation = new Vector3(0, 97, 0);
    [SerializeField] protected float m_timer = 1f;

    private Vector3 m_initialPosition;
    private Vector3 m_initialRotation;
    private Tweener m_tween;
    private bool m_isOpened;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
        m_initialPosition = transform.localPosition;
        m_initialRotation = transform.eulerAngles;
    }

    public override void Use() { }

    protected void Open()
    {
        m_tween?.Kill();

        if (m_isOpened)
        {
            m_tween = transform.DOLocalMove(m_initialPosition, m_timer);
            m_tween = transform.DORotate(m_initialRotation, m_timer);
        }
        else
        {
            m_tween = transform.DOLocalMove(m_initialPosition - m_newPosition, m_timer);
            m_tween = transform.DOBlendableRotateBy(-m_newRotation, m_timer);
        }

        m_isOpened = !m_isOpened;
    }
}
