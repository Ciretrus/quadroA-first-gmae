using System.Collections.Generic;
using UnityEngine;

public class DrawingRuneController : MonoBehaviour
{
    [SerializeField] private DrawableLine[] m_drawableLines;
    [SerializeField] private UIController m_uiController;

    private string[] m_runeNames;

    private void OnEnable()
    {
        m_runeNames = new string[m_drawableLines.Length];

        foreach (var line in m_drawableLines)
        {
            line.HasDrawnSymbol += CompareDrawing;
            line.HasStartedDrawing += HideText;
        }
        
        for(int i = 0; i < m_drawableLines.Length; i++)
        {
            m_runeNames[i] = m_drawableLines[i].runeName;
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

    private void CompareDrawing((string, List<Vector3>)[] originals, List<Vector3> points)
    {
        List<Vector3> normalizedPoints = UnistrokeRecognizer.GetNormalizedPoints(points); 

        string name = GetProperFigureName(originals, normalizedPoints);
        // TODO: Send name to a DrawableLine
        m_uiController.ShowFigureText(true, name);
    }

    private void HideText()
    {
        m_uiController.ShowFigureText(false, "");
    }

    private string GetProperFigureName((string, List<Vector3>)[] originals, List<Vector3> points)
    {
        string result = "";
        float previousDistance = 1f;

        for (int i = 0; i < originals.Length; i++)
        {
            float current = UnistrokeRecognizer.GetDistanceBetweenDraws(originals[i].Item2, points);

            if (current < previousDistance)
            {
                result = originals[i].Item1;
                previousDistance = current;
            }
        }

        return result;
    }
}
