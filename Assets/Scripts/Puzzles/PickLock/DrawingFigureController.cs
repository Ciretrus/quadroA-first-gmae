using System.Collections.Generic;
using UnityEngine;

public class DrawingFigureController : MonoBehaviour
{
    [SerializeField] private FigureDrawing[] m_figuresDrawings;
    [SerializeField] private UIController m_uiController;

    private void OnEnable()
    {
        foreach(var figure in m_figuresDrawings)
        {
            figure.HasDrawnSymbol += CompareDrawing;
            figure.HasStartedDrawing += HideText;
        }
    }

    private void OnDisable()
    {
        foreach (var figure in m_figuresDrawings)
        {
            figure.HasDrawnSymbol -= CompareDrawing;
            figure.HasStartedDrawing -= HideText;
        }
    }

    private void CompareDrawing((string, List<Vector3>)[] originals, List<Vector3> points)
    {
        List<Vector3> normalizedPoints = UnistrokeRecognizer.GetNormalizedPoints(points); 

        string name = GetProperFigureName(originals, normalizedPoints);
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
