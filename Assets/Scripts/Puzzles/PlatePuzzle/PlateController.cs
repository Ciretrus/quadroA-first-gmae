using UnityEngine;

public class PlateController : MonoBehaviour
{
    [SerializeField] Material m_inactiveMaterial;
    [SerializeField] Material m_activeMaterial;
    [SerializeField] Pushable[] m_plates;

    private Renderer[] m_renderers;
    private int[] m_correctSequence = { 1, 1, 3, 5 };
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
    }

    public void AddToSequence(Pushable plate)
    {
        int correctIndex = m_correctSequence[m_currentStep] - 1;
        if (plate == m_plates[correctIndex])
        {
            m_renderers[correctIndex].material = m_activeMaterial;
            m_currentStep++;
        }
        else
        {
            ResetPlates();
            return;
        }

        if (m_currentStep == m_correctSequence.Length)
        {
            ChangeMaterials(m_activeMaterial);
            // TODO Play the complete compostion
            m_isSolved = true;
        }
    }

    private void ResetPlates()
    {
        ChangeMaterials(m_inactiveMaterial);
        m_currentStep = 0;
    }

    private void ChangeMaterials(Material material)
    {
        foreach (Renderer renderer in m_renderers)
        {
            renderer.material = material;
        }
    }
}