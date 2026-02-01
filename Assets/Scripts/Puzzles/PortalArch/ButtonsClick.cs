using System.Collections;
using UnityEngine;
using Puzzles;

public class ButtonsClick : Usable
{
    [SerializeField] private RuneSwitch[] m_runeActivations;
    [SerializeField] private BasePuzzle m_puzzle;
    [SerializeField] private Vector3 m_clickedShiftPosition;
    [SerializeField] private float m_timeClick = 0.3f;
    
    private bool m_clicked = false;
    private Vector3 m_startPosition;
    private Vector3 m_newPosition;

    public void Awake()
    {
        Initialize(UsableType.NonBlocking);
        m_startPosition = transform.position;
        m_newPosition = m_startPosition - m_clickedShiftPosition;
    }
    
    public override void Use()
    {
        if (!m_clicked)
        {
            StartCoroutine(Click());
            for (int i = 0; i < m_runeActivations.Length; i++)
            {
                m_runeActivations[i].ChangeState();
            }
            m_puzzle.CheckCondition();
        }
    }
    
    private IEnumerator Click()
    {
        m_clicked = true;
        Coroutine coroutine = StartCoroutine(ChangePosition(m_newPosition));
        yield return coroutine;
        coroutine = StartCoroutine(ChangePosition(m_startPosition));
        yield return coroutine;
        m_clicked = false;
    }

    private IEnumerator ChangePosition(Vector3 newPosition)
    {
        float t = 0;
        Vector3 startPosition = transform.position;
        
        while (t < 1)
        {
            t += Time.deltaTime / m_timeClick;
            yield return new WaitForSeconds(Time.deltaTime);
            transform.position = Vector3.Lerp(startPosition, newPosition, t);
        }
    }
}
