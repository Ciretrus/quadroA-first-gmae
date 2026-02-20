using Puzzles;
using System;
using UnityEngine;

public class RuneDraw : Usable, ICondition
{
    public event Action<RuneDraw> DisableDrawing;

    [SerializeField] private BasePuzzle m_puzzle;
    [SerializeField] private DrawableLine m_drawableLine;
    [SerializeField] private RuneSwitch m_rune;

    public DrawableLine drawableLine { get { return m_drawableLine; } }

    public bool IsSolved => !enabled;

    private void OnEnable()
    {
        Initialize(UsableType.Blocking);

        if (m_drawableLine != null)
        {
            m_drawableLine.Solved += SolveDrawing;
        }
    }

    private void OnDisable()
    {
        if (m_drawableLine != null)
        {
            m_drawableLine.Solved -= SolveDrawing;
        }
    }

    public override void Use()
    {
        m_drawableLine.canDraw = !m_drawableLine.canDraw;
    }

    private void SolveDrawing()
    {
        m_rune.ColorIn();
        DisableDrawing.Invoke(this); 
        enabled = false;
        m_puzzle.CheckCondition();
    }
}
