using UnityEngine;

public class RuneDraw : Usable
{
    [SerializeField] private DrawableLine m_drawableLine;

    private void OnEnable()
    {
        Initialize(UsableType.Blocking);

        m_drawableLine.onSolved += DisableDrawing;
    }

    private void OnDisable()
    {
        m_drawableLine.onSolved -= DisableDrawing;
    }

    public override void Use()
    {
        m_drawableLine.canDraw = !m_drawableLine.canDraw;
    }

    private void DisableDrawing()
    {
        Use();
        // TODO: Disable the ability to draw
        // move camera from rune view to normal
        // disable script
    }
}
