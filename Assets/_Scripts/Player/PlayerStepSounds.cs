using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerStepSounds : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private List<AudioClip> m_sounds;
    [SerializeField] private float m_delay = 0.5f;
    [SerializeField] private float m_sprintDelay = 0.3f;
    [SerializeField] private float m_sneakDelay = 0.7f;

    private PlayerController m_controller;
    private AudioClip m_currentClip;
    private float m_currentVolume = 1.0f;
    private float m_currentDelay;
    private bool m_isPlaying = false;

    private void Start()
    {
        m_controller = ServiceLocator.Resolve<PlayerController>();
        m_currentDelay = m_delay;
    }

    private void Update()
    {
        if (m_controller.isSprinting)
        {
            m_currentDelay = m_sprintDelay;
        }
        else if (m_controller.isSneaking)
        {
            m_currentDelay = m_sneakDelay;
        }
        else
        {
            m_currentDelay = m_delay;
        }

        if (m_controller.isGrounded && m_controller.isMoving && !m_isPlaying)
        {
            int randomIndex = Random.Range(0, m_sounds.Count);
            AudioClip sound = m_sounds[randomIndex];
            PlaySound(sound);
        }
    }

    public void PlaySound(AudioClip sound, float volume = 0.1f, bool destroyed = false, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        m_currentClip = sound;
        m_currentVolume = volume;
        m_audioSource.pitch = Random.Range(minPitch, maxPitch);
        StartCoroutine(PlaySoundWithDelay());
    }

    private IEnumerator PlaySoundWithDelay()
    {
        m_isPlaying = true;
        m_audioSource.PlayOneShot(m_currentClip, m_currentVolume);
        yield return new WaitForSeconds(m_currentDelay);
        m_isPlaying = false;
    }
}
