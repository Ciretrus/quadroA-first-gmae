using System;
using UnityEngine;

public class DiaryInteractable : MonoBehaviour, ISoundEvent
{
    [SerializeField] private DiaryInteractableData m_diaryInteractable;
    public bool wasTriggered { get; private set; }

    public event Action DiaryTrigger;

    public void TriggerDiaryRecord()
    {
        wasTriggered = true;
        DiaryTrigger?.Invoke();
        ServiceLocator.Resolve<Diary>().SetPage(m_diaryInteractable.sprite, m_diaryInteractable.filename);
    }
}