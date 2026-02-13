using System;
using UnityEngine;

public class RuneDraw : Usable
{
    public event Action<RuneDraw> OnDisableDrawing;

    [SerializeField] private DrawableLine m_drawableLine;

    public DrawableLine drawableLine { get { return m_drawableLine; } }

    private void OnEnable()
    {
        Initialize(UsableType.Blocking);

        if (m_drawableLine != null)
        {
            m_drawableLine.m_onSolved += DisableDrawing;
        }
    }

    private void OnDisable()
    {
        if (m_drawableLine != null)
        {
            m_drawableLine.m_onSolved -= DisableDrawing;
        }
    }

    public override void Use()
    {
        m_drawableLine.canDraw = !m_drawableLine.canDraw;
    }

    private void DisableDrawing()
    {
        OnDisableDrawing.Invoke(this); 
        enabled = false;
    }
}
