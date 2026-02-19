using DG.Tweening;
using Puzzles;
using UnityEngine;

public class TrinketBoxCap : MonoBehaviour
{
    [SerializeField] private BasePuzzle m_puzzle;
    [SerializeField] private Vector3 m_newRotation = new Vector3(-60, 0, 0);
    [SerializeField] protected float m_timer = 1.5f;

    private void OnEnable()
    {
        if (m_puzzle != null)
        {
            m_puzzle.Solved += OpenTrinketBox;
        }
    }

    private void OnDisable()
    {
        if (m_puzzle != null)
        {
            m_puzzle.Solved -= OpenTrinketBox;
        }
    }

    private void OpenTrinketBox()
    {
        transform.DOBlendableLocalRotateBy(m_newRotation, m_timer);
    }
}
