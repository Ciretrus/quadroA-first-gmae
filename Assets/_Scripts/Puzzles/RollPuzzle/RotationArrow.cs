using UnityEngine;
using Puzzles;

public class RotationArrow : Usable
{
    [SerializeField] private RollRotation m_roll;
    [SerializeField][Range(-1, 1)] private int m_direction;

    private void Awake()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        m_roll.Rotate(m_direction);
    }
}