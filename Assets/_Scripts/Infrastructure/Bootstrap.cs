using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private PlayerController m_playerController;
        [SerializeField] private ItemsActivations m_itemsActivations;
        [SerializeField] private Camera m_camera;
        [SerializeField] private Inventory m_inventory;
        [SerializeField] private ThiefEye m_thiefEye;
        [SerializeField] private Diary m_diary;

        private void Awake()
        {
            ServiceLocator.Register(m_playerController);
            ServiceLocator.Register(m_itemsActivations);
            ServiceLocator.Register(m_camera);
            ServiceLocator.Register(m_inventory);
            ServiceLocator.Register(m_thiefEye);
            ServiceLocator.Register(m_diary);
        }
    }
}