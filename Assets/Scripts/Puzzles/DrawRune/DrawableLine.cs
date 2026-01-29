using Puzzles;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class DrawableLine : BasePuzzle
{
    public event Action<(string, List<Vector3>)[], List<Vector3>> HasDrawnSymbol;
    public event Action HasStartedDrawing;

    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private string m_runeName;
    [SerializeField] private int m_figuresAmount = 10;
    [SerializeField] private float m_minDistance = 0.1f;
    // Fill with original drawings and names
    // Change List to an array
    [SerializeField] private (string, List<Vector3>)[] m_originalsTuples;
    
    // Remove m_originals
    private List<List<Vector3>> m_originals;
    private List<Vector3> m_dotsList;
    private bool m_canDraw;
    private bool m_hasBrush => Inventory.instance.HasItem(GlobalConstants.RollBrush);

    public string runeName 
    { 
        get => m_runeName; 
        private set => m_runeName = value; 
    }

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
        if (m_canDraw && m_hasBrush) DrawLine();
    }

    public override void CheckCondition()
    {
        // TODO: Get a name of a drawn rune and check if it's this rune name
        if (m_runeName == m_originalsTuples[0].Item1)
        {
            NotifySolved();
        }
    }

    private void DrawLine()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            HasStartedDrawing?.Invoke();

            Vector3 mousePixelPos = Input.mousePosition;
            mousePixelPos.z = Camera.main.nearClipPlane + Camera.main.nearClipPlane * 0.01f;

            Vector3 mouseWorldPos = Camera.main.ScreenToViewportPoint(mousePixelPos);
            mouseWorldPos.x = - mouseWorldPos.x;
            mouseWorldPos.x += 0.5f;
            mouseWorldPos.y -= 0.5f;
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
                // TODO Remove that if statement, it's for adding originals first
                if (m_originals.Count < m_figuresAmount)
                {
                    m_originals.Add(UnistrokeRecognizer.GetNormalizedPoints(m_dotsList)); 

                    int index = m_originals.Count - 1;
                    m_originalsTuples[index] = (Convert.ToString(index), m_originals[index]);
                }
                else
                {
                    HasDrawnSymbol?.Invoke(m_originalsTuples, m_dotsList);
                    CheckCondition();
                }
            }

            // TODO: Get originals - Lists of <Vector3>
            /*m_dotsList.Clear();
            m_lineRenderer.positionCount = 0;*/
        }
    }

    private void DrawFigure(List<Vector3> points)
    {
        m_lineRenderer.positionCount = points.ToArray().Length;
        m_lineRenderer.SetPositions(points.ToArray());
    }
}
