using UnityEngine;

public class Svitok : Usable
{
    [SerializeField] FigureDrawing m_figureDrawing;

    private void OnEnable()
    {
        Initialize(UsableType.Blocking);
    }

    public override void Use()
    {
        m_figureDrawing.canDraw = !m_figureDrawing.canDraw;
    }
}
