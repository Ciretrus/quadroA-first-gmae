using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] private Material m_inactiveMaterial;
    [SerializeField] private Material m_activeMaterial;
    [SerializeField] private Pushable[] m_plates;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private float m_delay = 0.3f;

    private Renderer[] m_renderers;
    private AudioClip[] m_composition;
    private int[] m_correctSequence = { 1, 1, 3, 5, 1 };
    private int m_currentStep = 0;
    private bool m_isSolved = false;

    public bool isSolved => m_isSolved;

    private void Awake()
    {
        m_renderers = new Renderer[m_plates.Length];

        for (int i = 0; i < m_plates.Length; i++)
        {
            m_renderers[i] = m_plates[i].GetComponent<Renderer>();
        }

        m_composition = new AudioClip[m_correctSequence.Length];
    }

    public void AddToSequence(Pushable plate)
    {
        m_audioSource.PlayOneShot(plate.audioClip);

        int correctIndex = m_correctSequence[m_currentStep] - 1;
        if (plate == m_plates[correctIndex])
        {
            m_currentStep++;

            for (int i = 0; i < m_composition.Length - 1; i++)
            {
                m_composition[i] = m_composition[i + 1];
            }
            m_composition[m_composition.Length - 1] = plate.audioClip;
        }
        else
        {
            m_currentStep = 0;
            return;
        }

        if (m_currentStep == m_correctSequence.Length)
        {
            ChangeMaterials(m_activeMaterial);
            StartCoroutine(PlaySoundWithDelay());
            m_isSolved = true;
        }
    }

    private IEnumerator PlaySoundWithDelay()
    {
        foreach(AudioClip audio in m_composition)
        {
            m_audioSource.PlayOneShot(audio);
            yield return new WaitForSeconds(m_delay);
        }
    }

    private void ChangeMaterials(Material material)
    {
        foreach (Renderer renderer in m_renderers)
        {
            renderer.material = material;
        }
    }
}