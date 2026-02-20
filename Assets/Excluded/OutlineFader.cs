using System.Collections;
using UnityEngine;

public class OutlineFader : MonoBehaviour
{
    [SerializeField][Range(0.001f, 0.05f)] private float m_deltaTime = 0.01f;
    [SerializeField][Range(0.001f, 0.05f)] private float m_step = 0.01f;
    [SerializeField][Range(0, 10f)] private float m_outlineMaxWidth = 10f;    

    public void Enable(Outline outline)
    {
        StartCoroutine(EnableCoroutine(outline));
    }

    public void Disable(Outline outline)
    {
        StartCoroutine(DisableCoroutine(outline));
    }

    private IEnumerator EnableCoroutine(Outline outline)
    {        
        outline.enabled = true;
        for (float i = 0; i < m_outlineMaxWidth; i+=m_step)
        {
            outline.OutlineWidth = i;
            yield return new WaitForSeconds(m_deltaTime);
        }
    }

    private IEnumerator DisableCoroutine(Outline outline)
    {        
        for (float i = m_outlineMaxWidth; i > 0; i-=m_step)
        {
            outline.OutlineWidth = i;
            yield return new WaitForSeconds(m_deltaTime);
        }
        outline.enabled = false;
    }
}
