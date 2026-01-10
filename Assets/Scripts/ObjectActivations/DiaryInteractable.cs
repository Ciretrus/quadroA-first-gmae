using System;
using UnityEngine;

public class DiaryInteractable : MonoBehaviour
{
    public event Action<Sprite, string> Triggered;

    [SerializeField] private DiaryInteractableBase m_diaryInteractable;

    public bool wasTriggered => m_diaryInteractable.wasTriggered;

    public void TriggerDiaryRecord()
    {
        m_diaryInteractable.wasTriggered = true;
        Triggered?.Invoke(m_diaryInteractable.sprite, m_diaryInteractable.filename);
    }
}