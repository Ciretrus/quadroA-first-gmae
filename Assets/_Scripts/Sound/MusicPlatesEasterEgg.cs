using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlatesEasterEgg : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;    

    private void OnValidate()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        m_audioSource.Play();
    }
}
