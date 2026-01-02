using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class FigureDrawing : MonoBehaviour
{
    public event Action<(string, List<Vector3>)[], List<Vector3>> HasDrawnSymbol;
    public event Action HasStartedDrawing;

    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private int m_figuresAmount = 10;
    [SerializeField] private float m_minDistance = 0.1f;

    private (string, List<Vector3>)[] m_originalsTuples;
    private List<List<Vector3>> m_originals;
    private List<Vector3> m_dotsList;
    private bool m_canDraw;

    public bool canDraw
    {
        get => m_canDraw; 
        set => m_canDraw = value;
    }

    private void Start()
    {
        m_originalsTuples = new (string, List<Vector3>)[m_figuresAmount];
        m_originals = new List<List<Vector3>>();
        m_dotsList = new List<Vector3>();
    }

    private void Update()
    {
        if (m_canDraw) DrawLine();
    }

    private void DrawLine()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            HasStartedDrawing?.Invoke();

            // TODO Fix line position
            Vector3 mousePixelPos = Input.mousePosition;
            mousePixelPos.z = Camera.main.nearClipPlane + Camera.main.nearClipPlane * 0.01f;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePixelPos);
            mouseWorldPos.x = - mouseWorldPos.x;
            mouseWorldPos.y -= 1.25f;
            mouseWorldPos.z = 0f;

            if (m_dotsList.Count > 0)
            {
                float distance = Vector3.Distance(m_dotsList[m_dotsList.Count - 1], mouseWorldPos);

                if (distance > m_minDistance)
                {
                    m_dotsList.Add(mouseWorldPos);
                }
            }
            else
            {
                m_dotsList.Add(mouseWorldPos);
            }

            DrawFigure(m_dotsList);
        }
        else
        {
            if (m_dotsList.Count > 0)
            {
                if (m_originals.Count < m_figuresAmount)
                {
                    m_originals.Add(UnistrokeRecognizer.GetNormalizedPoints(m_dotsList)); 

                    int index = m_originals.Count - 1;
                    m_originalsTuples[index] = (Convert.ToString(index), m_originals[index]);
                }
                else
                {
                    HasDrawnSymbol?.Invoke(m_originalsTuples, m_dotsList);
                }
            }

            m_dotsList.Clear();
            m_lineRenderer.positionCount = 0;
        }
    }

    private void DrawFigure(List<Vector3> points)
    {
        m_lineRenderer.positionCount = points.ToArray().Length;
        m_lineRenderer.SetPositions(points.ToArray());
    }
}
