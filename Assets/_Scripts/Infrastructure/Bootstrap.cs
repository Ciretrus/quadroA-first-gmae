using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour 
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private Inventory m_inventory;

        private void Awake()
        {
            // move registration here from PlayerController
            ServiceLocator.Register(m_camera);
            ServiceLocator.Register(m_inventory);
        }
    }
}