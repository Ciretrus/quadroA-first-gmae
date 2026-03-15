using Puzzles;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActionEventSound : InteractableSound
{
    private ISoundEvent m_soundEvent;

    private void OnValidate()
    {        
        ISoundEvent soundEvent;
        if (gameObject.TryGetComponent(out soundEvent))
        {
            m_soundEvent = soundEvent;
        }
    }

    private void Awake()
    {
        if (m_soundEvent == null)
        {
            TryGetComponent(out m_soundEvent);
        }
    }

    private void OnEnable()
    {
        m_soundEvent.DiaryTrigger += PlaySound;
    }

    private void OnDisable()
    {
        m_soundEvent.DiaryTrigger -= PlaySound;
    }    
}
