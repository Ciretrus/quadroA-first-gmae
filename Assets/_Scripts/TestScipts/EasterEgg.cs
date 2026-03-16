using DG.Tweening;
using Puzzles;
using Puzzles.PortalArch;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    [SerializeField] private BasePuzzle m_plateController;
    [SerializeField] private GameObject m_gameObject;
    [SerializeField] private GameObject m_door;
    [SerializeField] private Vector3 m_newPos = new Vector3(0,1,1);
    [SerializeField] private float m_time = 1f;
    

    private void OnEnable()
    {
        m_plateController.Solved += ActivateEgg;
    }
    private void OnDisable()
    {
        m_plateController.Solved -= ActivateEgg;
    }
    public void ActivateEgg()
    {
        m_gameObject.SetActive(true);
        m_door.transform.DOLocalMove(m_newPos, m_time).SetEase(Ease.InQuad);

    }
    
}
