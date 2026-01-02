using UnityEngine;

namespace NonEuclidian
{
    public class PortalCamera : MonoBehaviour
    {
        [SerializeField] private Transform m_playerCameraPosition;
        [SerializeField] private Transform m_portalPosition;
        [SerializeField] private Transform m_otherPortalPosition;

        void LateUpdate()
        {
            // 1. позиция игрока в ЛОКАЛЬНЫХ координатах другого портала
            Vector3 localPos =
                m_otherPortalPosition.InverseTransformPoint(
                    m_playerCameraPosition.position
                );

            // 2. перенос в пространство текущего портала
            transform.position =
                m_portalPosition.TransformPoint(localPos);

            // 3. разница поворотов порталов
            Quaternion rotationDiff =
                m_portalPosition.rotation *
                Quaternion.Inverse(m_otherPortalPosition.rotation);

            // 4. применяем к камере игрока
            transform.rotation =
                rotationDiff * m_playerCameraPosition.rotation;
        }
    }
}
