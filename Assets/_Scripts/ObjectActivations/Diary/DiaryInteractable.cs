using UnityEngine;

public class DiaryInteractable : MonoBehaviour
{
    [SerializeField] private DiaryInteractableData m_diaryInteractable;

    public bool wasTriggered { get; private set; }

    public void TriggerDiaryRecord()
    {
        wasTriggered = true;
        ServiceLocator.Resolve<Diary>().SetPage(m_diaryInteractable.sprite, m_diaryInteractable.filename);
    }
}