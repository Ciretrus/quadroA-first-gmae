using Puzzles;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActionEventSound : InteractableSound
{
    private ISoundEvent m_soundEvent;

    private void OnValidate()
    {
        if (m_source == null)
        {
            TryGetComponent<AudioSource>(out m_source);
        }
        ISoundEvent soundEvent;
        if (gameObject.TryGetComponent<ISoundEvent>(out soundEvent))
        {
            m_soundEvent = soundEvent;
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
