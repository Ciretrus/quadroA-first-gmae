using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ThiefEye : MonoBehaviour
{
    [SerializeField] private LayerMask m_layerThiefEye;
    [SerializeField] private Material m_material;
    [SerializeField] private float m_activationTime = 0.3f;
    [SerializeField] private float m_durationTime =3f;
    [SerializeField] private AudioSource m_audio;
    
    private Camera m_camera;
    private Volume[] m_volumes;
    private Volume m_standardVolume;
    private Volume m_thiefEyeVolume;
    
    private bool m_hasStarted = false;
    private int m_originalCullingMask;

    private const float m_maxMaterialValue = 2f;
    private const float m_minMaterialValue = 0f;

    public LayerMask layerThiefEye => m_layerThiefEye;
    public bool hasStarted => m_hasStarted;

    private void Start()
    {
        m_camera = ServiceLocator.Resolve<Camera>();

        m_originalCullingMask = m_camera.cullingMask;
        m_volumes = m_camera.GetComponents<Volume>();
        m_standardVolume = m_volumes[0];
        m_thiefEyeVolume = m_volumes[1];
    }

    public void ActivateThiefEye(InputAction.CallbackContext context)
    {
        if (!m_hasStarted)
        {
            StartCoroutine(CastThiefEyeSkill(m_durationTime));
            m_audio.Play();
        }
    }

    private IEnumerator CastThiefEyeSkill(float duration)
    {
        yield return StartCoroutine(ActivateThiefEyeEffect(true));
       m_camera.cullingMask |= m_layerThiefEye;

        yield return new WaitForSeconds(duration);
       m_camera.cullingMask = m_originalCullingMask;
        yield return StartCoroutine(ActivateThiefEyeEffect(false));
    }

    private IEnumerator ActivateThiefEyeEffect(bool turnOn)
    {
        m_hasStarted = true;
        float materialValue = 0f;
        for (float i = 0; i < 1; i += Time.deltaTime / m_activationTime)
        {
            if (turnOn)
            {
                materialValue = Mathf.InverseLerp(0, 1f, i);
                materialValue = Mathf.Lerp(m_minMaterialValue, m_maxMaterialValue, materialValue);
                print(materialValue);
                m_material.SetFloat("_value", materialValue);
                m_standardVolume.weight = 1f - i;
                m_thiefEyeVolume.weight = i;
            }
            else
            {
                materialValue = Mathf.InverseLerp(0, 1f, i);
                materialValue = Mathf.Lerp(m_minMaterialValue, m_maxMaterialValue, materialValue);
                m_material.SetFloat("_value", m_maxMaterialValue - materialValue);
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

        m_hasStarted = turnOn;
    }
}