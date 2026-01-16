using UnityEngine;

public class InteractableSound : MonoBehaviour
{
    [SerializeField] protected AudioClip[] m_sounds;
    [SerializeField] protected AudioSource m_source;

    public void PlaySound()
    {
        m_source.PlayOneShot(GetRandomSound());
    }

    public void PlayPitchedSound(float minPitch = 0.8f, float maxPitch = 1.2f)
    {        
        m_source.pitch = Random.Range(minPitch, maxPitch);
        m_source.PlayOneShot(GetRandomSound());
    }

    protected AudioClip? GetRandomSound()
    {
        if (m_sounds.Length == 0)
            return null;

        return m_sounds[Random.Range(0, m_sounds.Length)];
    }
}
