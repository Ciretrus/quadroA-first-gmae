using System;
using UnityEngine;

public class DiaryInteractable : MonoBehaviour
{
    [SerializeField] private DiaryInteractableBase m_diaryInteractable;

    public bool wasTriggered => m_diaryInteractable.wasTriggered;

    public void TriggerDiaryRecord()
    {
        m_diaryInteractable.wasTriggered = true;
        Diary.instance.SetPage(m_diaryInteractable.sprite, m_diaryInteractable.filename);
    }
}