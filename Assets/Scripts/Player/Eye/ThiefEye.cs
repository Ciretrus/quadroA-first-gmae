using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ThiefEye : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private LayerMask m_layerThiefEye;
    [SerializeField] private float m_activationTime = 0.3f;
    [SerializeField] private float m_durationTime =3f;

    [Header("PostProcessing")]
    private Volume[] m_volumes;
    private Volume m_standardVolume;
    private Volume m_thiefEyeVolume;
    
    private bool m_isStarted = false;
    private int m_originalCullingMask;

    private void Awake()
    {
        m_originalCullingMask = m_camera.cullingMask;
        m_volumes = m_camera.GetComponents<Volume>();
        m_standardVolume = m_volumes[0];
        m_thiefEyeVolume = m_volumes[1];
    }

    public void ActivateThiefEye(InputAction.CallbackContext context)
    {
        if (!m_isStarted)
        {
            StartCoroutine(ThiefEyeSkill(m_durationTime));
        }
    }

    private IEnumerator ThiefEyeSkill(float duration)
    {
        yield return StartCoroutine(ActivationThiefEyeEffect(true));
        m_camera.cullingMask |= m_layerThiefEye;

        yield return new WaitForSeconds(duration);
        m_camera.cullingMask = m_originalCullingMask;
        yield return StartCoroutine(ActivationThiefEyeEffect(false));
    }

    private IEnumerator ActivationThiefEyeEffect(bool turnOn)
    {
        m_isStarted = true;

        for (float i = 0; i < 1; i += Time.deltaTime / m_activationTime)
        {
            if (turnOn)
            {
                m_standardVolume.weight = 1f - i;
                m_thiefEyeVolume.weight = i;
            }
            else
            {
                m_thiefEyeVolume.weight = 1f - i;
                m_standardVolume.weight = i;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        if (turnOn)
        {
            m_standardVolume.weight = 0;
            m_thiefEyeVolume.weight = 1;
        }
        else
        {
            m_thiefEyeVolume.weight = 0;
            m_standardVolume.weight = 1;
        }

        m_isStarted = turnOn;
    }
}