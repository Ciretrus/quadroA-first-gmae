using System.Collections.Generic;
using UnityEngine;

public class DrawingRuneController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_flashParticleSystem;
    [SerializeField] private DrawableLine[] m_drawableLines;
    [SerializeField] private UIController m_uiController;
    [SerializeField] private RuneData[] m_runes;

    private RuneData m_currentRune;

    public RuneData currentRune { set { m_currentRune = value; } }

    private void OnEnable()
    {
        foreach (var line in m_drawableLines)
        {
            line.DrawnSymbol += CompareDrawing;
        }

        m_flashParticleSystem = Instantiate(m_flashParticleSystem);
    }

    private void OnDisable()
    {
        foreach (var line in m_drawableLines)
        {
            line.DrawnSymbol -= CompareDrawing;
        }
    }

    private void CompareDrawing(List<Vector3> points, Transform transform)
    {
        List<Vector3> normalizedPoints = UnistrokeRecognizer.GetNormalizedPoints(points); 

        string name = GetDrawnRuneName(normalizedPoints);

        foreach (var rune in m_runes)
        {
            if (rune.runeName == name && rune == m_currentRune)
            {
                rune.solved = true;
                return;
            }
        }

        m_flashParticleSystem.transform.position = transform.position;
        m_flashParticleSystem.Play();
    }

    private string GetDrawnRuneName(List<Vector3> points)
    {
        string result = "";
        float previousDistance = 1f;

        for (int i = 0; i < m_runes.Length; i++)
        {
            float current = UnistrokeRecognizer.GetDistanceBetweenDraws(m_runes[i].original, points);

            if (current < previousDistance)
            {
                result = m_runes[i].runeName;
                previousDistance = current;
            }
        }

        return result;
    }
}
