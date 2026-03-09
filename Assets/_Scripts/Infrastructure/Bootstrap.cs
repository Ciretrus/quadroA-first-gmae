using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private PlayerController m_playerController;
        [SerializeField] private ItemsActivations m_itemsActivations;
        [SerializeField] private Camera m_camera;
        [SerializeField] private UIController m_uiController;
        [SerializeField] private Inventory m_inventory;
        [SerializeField] private ThiefEye m_thiefEye;
        [SerializeField] private Diary m_diary;
        [SerializeField] private DiaryNotificationSystem m_notificationSystem;
        [SerializeField] private DrawingRuneController m_drawingRuneController;
        [SerializeField] private PlateController m_plateController;

        private void Awake()
        {
            ServiceLocator.ClearServices();

            ServiceLocator.Register(m_playerController);
            ServiceLocator.Register(m_itemsActivations);
            ServiceLocator.Register(m_camera);
            ServiceLocator.Register(m_uiController);
            ServiceLocator.Register(m_inventory);
            ServiceLocator.Register(m_thiefEye);
            ServiceLocator.Register(m_diary);
            ServiceLocator.Register(m_notificationSystem);
            ServiceLocator.Register(m_drawingRuneController);
            ServiceLocator.Register(m_plateController);
        }
    }
}