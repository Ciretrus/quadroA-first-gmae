using System.Collections;
using UnityEngine;

public class RuneSwitch : MonoBehaviour
{
    [SerializeField] private Color m_colorOff = Color.black;
    [SerializeField] private Color m_colorOn = Color.cyan;
    [SerializeField] private float m_glowTime = 1.0f;
    private MaterialPropertyBlock m_emissionMat;
    private MeshRenderer m_meshRenderer;
    private Coroutine m_coroutine;
    private bool m_isStarted = false;
    private bool m_isActive = false;
    
    public bool isActive
    {
        get { return m_isActive; }
    }

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_emissionMat = new MaterialPropertyBlock();
    }

    public bool ChangeState() 
    {
        if (m_coroutine != null) StopCoroutine(m_coroutine);  
        
        m_isStarted = true;
        
        if (m_isActive)
        {
            m_isActive = false;
            m_coroutine = StartCoroutine(StartGlowing(m_colorOff, 1));
        }
        else
        {
            m_isActive = true;
            m_coroutine = StartCoroutine(StartGlowing(m_colorOn, 1));
        }
        return true;
    } 

    private IEnumerator StartGlowing(Color color,float intensity)
    {   
        m_isStarted = true;
        Coroutine coroutine = StartCoroutine(SwitchColor(color, intensity));
        yield return coroutine;
        m_isStarted=false;
    }

    private IEnumerator SwitchColor(Color color,float intensity)
    {
        Color lastColor = m_emissionMat.GetColor("_EmissionColor");
        Color expectedColor = color;
        Color differentColor = new Color(0, 0, 0);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / m_glowTime;
            differentColor = Color.Lerp(lastColor, expectedColor, t);
            m_emissionMat.SetColor("_EmissionColor", differentColor * intensity);
            m_meshRenderer.SetPropertyBlock(m_emissionMat, 1);
            yield return new WaitForSeconds(Time.deltaTime);
        } 
    }
}
