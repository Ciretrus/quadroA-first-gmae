using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActionEventSound : InteractableSound
{
    private ISoundEvent m_soundEvent;

    private void Awake()
    {
        if (m_source == null)
        {
            TryGetComponent(out m_source);
        }

        if (gameObject.TryGetComponent(out ISoundEvent soundEvent))
        {
            m_soundEvent = soundEvent;
        }
    }

    private void OnEnable()
    {
        m_soundEvent.TriggerSound += PlaySound;
    }

    private void OnDisable()
    {
        m_soundEvent.TriggerSound -= PlaySound;
    }
}
