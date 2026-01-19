using UnityEngine;
using Puzzles;

public class RotationArrow : Usable
{
    [SerializeField] private RollRotation m_roll;
    [SerializeField][Range(-1, 1)] private int m_direction;

    private void OnEnable()
    {
        Initialize(UsableType.NonBlocking);
    }

    public override void Use()
    {
        Debug.Log("activated");
        m_roll.Rotate(m_direction);
    }
}