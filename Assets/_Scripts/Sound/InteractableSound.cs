using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InteractableSound : MonoBehaviour
{
    [SerializeField] protected AudioClip[] m_sounds;
    [SerializeField] protected AudioSource m_source;

    private void OnEnable()
    {
        m_source = GetComponent<AudioSource>();
    }

    public AudioClip[] sounds => m_sounds;
    public AudioSource source => m_source;

    public void PlaySound()
    {
        m_source.PlayOneShot(GetRandomSound());
    }

    public void PlayPitchedSound(float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        Debug.LogWarning("Play Pitched");
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
}
