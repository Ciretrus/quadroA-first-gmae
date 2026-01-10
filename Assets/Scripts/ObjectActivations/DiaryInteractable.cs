using System;
using UnityEngine;

public class DiaryInteractable : MonoBehaviour
{
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private string m_filename;
    public event Action<Sprite, string> Triggered;

    public void TriggerDiaryRecord()
    {
        Triggered?.Invoke(m_sprite, m_filename);
    }
}