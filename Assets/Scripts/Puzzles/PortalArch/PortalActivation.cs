using Puzzles;
using UnityEngine;

public class PortalActivation : MonoBehaviour
{
    [SerializeField] private BasePuzzle m_puzzle;
    [SerializeField] private GameObject m_particle;


    private void OnEnable()
    {
        if (m_puzzle != null)
        {
            m_puzzle.m_onSolved += ActivatePortal;
        }
    }
    private void OnDisable()
    {
        if (m_puzzle != null)
        {
            m_puzzle.m_onSolved -= ActivatePortal;
        }
    }

    private void ActivatePortal()
    {
        m_particle.SetActive(true);
    }
}
