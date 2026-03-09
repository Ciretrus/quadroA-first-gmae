using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractableSound : MonoBehaviour
{
    [SerializeField] protected AudioClip[] m_sounds;
    [SerializeField] protected AudioSource m_source;
    [SerializeField] protected bool m_pitch = true;

    public AudioClip[] sounds => m_sounds;
    public AudioSource source => m_source;

    private void OnValidate()
    {
        if (m_source == null)
        {
            TryGetComponent<AudioSource>(out m_source);
        }
    }
    
    private void Awake()
    {
        if (m_source == null)
        {
            m_source = GetComponent<AudioSource>();
        }
    }

    public void PlaySound()
    {
        if (m_pitch)
        {
            PlayPitchedSound();
        }
        else
        {
            m_source.PlayOneShot(GetRandomSound());
        }            
    }

    public void PlayPitchedSound(float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        //Debug.LogWarning("Play Pitched");
        m_source.pitch = Random.Range(minPitch, maxPitch);
        m_source.PlayOneShot(GetRandomSound());
    }

#nullable enable
    protected AudioClip? GetRandomSound()
    {
        if (m_sounds.Length == 0)
            return null;

        return m_sounds[Random.Range(0, m_sounds.Length)];
    }

    public void ChangeSounds(params AudioClip[] newAudioClips)
    {
        m_sounds = newAudioClips;
    }
}
