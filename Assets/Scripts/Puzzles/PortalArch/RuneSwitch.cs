using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSwitch : MonoBehaviour
{

    private MaterialPropertyBlock m_emissionMat;
    private MeshRenderer m_meshRenderer;
    [SerializeField] private float m_glowTime = 1.0f;
    [SerializeField] private Color m_colorOff = Color.black;
    [SerializeField] private Color m_colorOn = Color.cyan;
    private bool m_isStarted = false;
    private bool m_isActive = false;
    private Coroutine coroutine;
    public bool isActive
    {
        get { return m_isActive; }
    }
    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_emissionMat = new MaterialPropertyBlock();
    }

    public bool changeState() 
    {       if (coroutine != null) StopCoroutine(coroutine);  
            m_isStarted = true;
            if (m_isActive)
            {
                m_isActive = false;
                coroutine = StartCoroutine(StartGlowing(m_colorOff, 1));
            }
            else 
            { 
                m_isActive = true;
                coroutine = StartCoroutine(StartGlowing(m_colorOn, 1));
            }
            return true;
        
    } 
    IEnumerator StartGlowing(Color color,float intensity)
    {   m_isStarted = true;
        Coroutine coroutine = StartCoroutine(switchColor(color, intensity));
        yield return coroutine;
        m_isStarted=false;
    }
    private IEnumerator switchColor(Color color,float intensity)
    {
        Color lastColor = m_emissionMat.GetColor("_EmissionColor");
        Color expectedColor = color;
        Color differentColor = new Color(0, 0, 0);
        float t = 0;
        while (t<1)
        {
            t += Time.deltaTime / m_glowTime;
            differentColor = Color.Lerp(lastColor, expectedColor, t);
            m_emissionMat.SetColor("_EmissionColor", differentColor * intensity);
            m_meshRenderer.SetPropertyBlock(m_emissionMat, 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        
            
    }
}
