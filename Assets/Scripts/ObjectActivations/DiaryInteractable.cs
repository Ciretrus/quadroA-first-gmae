using System;
using UnityEngine;

public class DiaryInteractable : MonoBehaviour
{
    [SerializeField] private DiaryInteractableBase m_diaryInteractable;
    public event Action<Sprite, string> Triggered;

    public void TriggerDiaryRecord()
    {
        Triggered?.Invoke(m_diaryInteractable.sprite, m_diaryInteractable.filename);
    }
}