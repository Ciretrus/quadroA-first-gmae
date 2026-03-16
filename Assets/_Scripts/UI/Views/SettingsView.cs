using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    [Header("Settings controllers")]
    [SerializeField] private Slider m_brightnessSlider;
    [SerializeField] private Slider m_soundSlider;
    [SerializeField] private TMP_Dropdown m_resolutionDropdown;
    [SerializeField] private Toggle m_windowModeToggle;

    [Header("Settings values")]
    [SerializeField] private float m_limitBrightnessValue;

    private ColorAdjustments m_colorAdjustments;

    private void OnEnable()
    {
        m_brightnessSlider.onValueChanged.AddListener(ChangeBrightness);
        m_soundSlider.onValueChanged.AddListener(ChangeSound);
        m_resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        m_windowModeToggle.onValueChanged.AddListener(ChangeWindowMode);
    }

    private void Start()
    {
        m_camera.GetComponent<Volume>().profile.TryGet(out m_colorAdjustments);
    }

    private void OnDisable()
    {
        m_brightnessSlider.onValueChanged.RemoveListener(ChangeBrightness);
        m_soundSlider.onValueChanged.RemoveListener(ChangeSound);
        m_resolutionDropdown.onValueChanged.RemoveListener(ChangeResolution);
        m_windowModeToggle.onValueChanged.RemoveListener(ChangeWindowMode);
    }

    private void ChangeBrightness(float value)
    {
        value -= m_limitBrightnessValue / 4f;
        value *= m_limitBrightnessValue * 2f;
        m_colorAdjustments.postExposure.value = value;
    }

    private void ChangeSound(float value)
    {

    }

    private void ChangeResolution(int value)
    {

    }

    private void ChangeWindowMode(bool value)
    {

    }
}
