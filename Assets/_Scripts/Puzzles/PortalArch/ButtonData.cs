using UnityEngine;

namespace Puzzles.PortalArch
{
    [CreateAssetMenu]
    public class ButtonData : ScriptableObject
    {
        [SerializeField] private Vector3 m_clickedShiftPosition;
        [SerializeField] private float m_timeClick = 0.3f;

        public Vector3 clickedShiftPosition => m_clickedShiftPosition;
        public float timeClick => m_timeClick;
    }
}
