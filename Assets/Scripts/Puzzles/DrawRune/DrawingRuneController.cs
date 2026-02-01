using System.Collections.Generic;
using UnityEngine;

public class DrawingRuneController : MonoBehaviour
{
    [SerializeField] private DrawableLine[] m_drawableLines;
    [SerializeField] private UIController m_uiController;
    [SerializeField] private RuneData[] m_runes;

    private RuneData m_currentRune;

    public RuneData currentRune { set { m_currentRune = value; } }

    private void OnEnable()
    {
        foreach (var line in m_drawableLines)
        {
            line.HasDrawnSymbol += CompareDrawing;
            line.HasStartedDrawing += HideText;
        }
    }

    private void OnDisable()
    {
        foreach (var line in m_drawableLines)
        {
            line.HasDrawnSymbol -= CompareDrawing;
            line.HasStartedDrawing -= HideText;
        }
    }

    private void CompareDrawing(List<Vector3> points)
    {
        List<Vector3> normalizedPoints = UnistrokeRecognizer.GetNormalizedPoints(points); 

        string name = GetDrawnRuneName(normalizedPoints);

        foreach (var rune in m_runes)
        {
            if (rune.runeName == name)
            {
                m_uiController.ShowFigureText(true, name);
                if (rune == m_currentRune) 
                { 
                    rune.solved = true; 
                }
                return;
            }
        }
        Debug.Log("no such a rune");
    }

    private void HideText()
    {
        m_uiController.ShowFigureText(false, "");
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
