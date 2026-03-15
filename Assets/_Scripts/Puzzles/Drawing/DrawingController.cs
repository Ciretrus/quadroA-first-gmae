using System.Collections.Generic;
using UnityEngine;

public class DrawingController : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_flashParticleSystem;
    [SerializeField] private AudioSource m_failAudioSource;
    [SerializeField] private DrawableLine[] m_drawableLines;
    [SerializeField] private DrawingData[] m_dataArray;
    [SerializeField] private DrawCanvas[] m_canvases;

    private DrawingData m_currentRune;

    public DrawingData currentRune { set { m_currentRune = value; } }
    public DrawCanvas[] canvases => m_canvases;

    private void OnEnable()
    {
        foreach (var line in m_drawableLines)
        {
            line.DrawnSymbol += CompareDrawing;
        }

        m_flashParticleSystem = Instantiate(m_flashParticleSystem);
    }

    private void OnDisable()
    {
        foreach (var line in m_drawableLines)
        {
            line.DrawnSymbol -= CompareDrawing;
        }
    }

    private void CompareDrawing(List<Vector3> points, Transform transform)
    {
        List<Vector3> normalizedPoints = UnistrokeRecognizer.GetNormalizedPoints(points); 

        string name = GetDrawnRuneName(normalizedPoints);

        foreach (var data in m_dataArray)
        {
            if (data.runeName == name && data == m_currentRune)
            {
                data.solved = true;
                return;
            }
        }

        m_flashParticleSystem.transform.position = transform.position;
        m_flashParticleSystem.Play();
        m_failAudioSource.Play();
    }

    private string GetDrawnRuneName(List<Vector3> points)
    {
        string result = "";
        float previousDistance = 3f;

        for (int i = 0; i < m_dataArray.Length; i++)
        {
            float current = UnistrokeRecognizer.GetDistanceBetweenDraws(m_dataArray[i].original, points);

            if (current < previousDistance)
            {
                result = m_dataArray[i].runeName;
                previousDistance = current;
            }
        }

        return result;
    }
}
