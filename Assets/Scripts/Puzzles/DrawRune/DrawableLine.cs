using Puzzles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class DrawableLine : BasePuzzle
{
    public event Action<List<Vector3>, Transform> HasDrawnSymbol;

    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private RuneData m_rune;
    [SerializeField] private float m_maxLength = 0.35f;
    [SerializeField] private float m_minDistance = 0.1f;
    
    private List<Vector3> m_dotsList;
    private bool m_canDraw;
    private bool m_hasBrush => Inventory.instance.HasItem(GlobalConstants.RollBrush);

    public RuneData rune {  get { return m_rune; } }

    public bool canDraw
    {
        get => m_canDraw; 
        set => m_canDraw = value;
    }

    private void Start()
    {
        m_dotsList = new List<Vector3>();
    }

    private void Update()
    {
        if (m_canDraw && m_hasBrush) DrawLine();
    }

    public override void CheckCondition()
    {
        if (m_rune.solved)
        {
            NotifySolved();
        }
    }

    private void DrawLine()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePixelPos = Input.mousePosition;
            mousePixelPos.z = Camera.main.nearClipPlane + Camera.main.nearClipPlane * 0.01f;

            Vector3 mouseWorldPos = Camera.main.ScreenToViewportPoint(mousePixelPos);
            mouseWorldPos.x = - mouseWorldPos.x;
            mouseWorldPos.x += 0.5f;
            mouseWorldPos.y -= 0.5f;
            mouseWorldPos.z = 0f;

            if (Math.Abs(mouseWorldPos.x) > m_maxLength || Math.Abs(mouseWorldPos.y) > m_maxLength)
            {
                ResetDrawing();
            }

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
            ResetDrawing();
        }
    }

    private void DrawFigure(List<Vector3> points)
    {
        m_lineRenderer.positionCount = points.ToArray().Length;
        m_lineRenderer.SetPositions(points.ToArray());
    }

    private void ResetDrawing()
    {
        if (m_dotsList.Count > 1)
        {
            if (!m_rune.solved)
            {
                HasDrawnSymbol?.Invoke(m_dotsList, transform);
            }
            CheckCondition();
        }

        m_dotsList.Clear();
        m_lineRenderer.positionCount = 0;
    }
}
