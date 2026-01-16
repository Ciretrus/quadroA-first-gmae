using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] Material m_inactiveMaterial;
    [SerializeField] Material m_activeMaterial;
    [SerializeField] List<Pushable> m_plates;

    private int[] m_correctSequence = { 1, 1, 3, 5 };
    private List<int> m_currentSequence = new List<int>();
    private int m_currentStep;
    private Renderer[] m_renderers;

    private void OnEnable()
    {
        for (int i = 0; i < m_plates.Count; i++)
        {
            m_renderers[i] = m_plates[i].GetComponent<Renderer>();
        }
    }

    private void AddToSequence(Pushable plate)
    {
        if (m_currentSequence.Count >= m_correctSequence.Length)
        {
            ChangeMaterials(m_inactiveMaterial);
            return;
        }

        m_currentStep = m_plates.IndexOf(plate) + 1;
        m_currentSequence.Add(m_currentStep);

        if (CheckCorrectSequence())
        {
            ChangeMaterials(m_activeMaterial);
            // TODO Play the complete compostion
            enabled = false;
        }
    }

    private bool CheckCorrectSequence()
    {
        return (m_currentSequence.Equals(m_correctSequence));
    }

    private void ChangeMaterials(Material material)
    {
        foreach (Renderer renderer in m_renderers)
        {
            renderer.material = material;
        }
    }
}