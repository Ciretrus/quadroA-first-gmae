using UnityEngine;

namespace NonEuclidian
{
    public class PlayerTeleportation : MonoBehaviour
    {
        [SerializeField] private CharacterController m_characterController;
        [SerializeField] private Transform m_reciever;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            Transform player = m_characterController.transform;

            // позиция игрока в локальных координатах портала
            Vector3 localPos =
                transform.InverseTransformPoint(player.position);

            // поворот игрока в локальных координатах портала
            Quaternion localRot =
                Quaternion.Inverse(transform.rotation) * player.rotation;

            m_characterController.enabled = false;

            // перенос в другой портал
            player.position =
                m_reciever.TransformPoint(localPos);

            // поворот относительно другого портала
            player.rotation =
                m_reciever.rotation * localRot;

            m_characterController.enabled = true;
        }
    }
}
