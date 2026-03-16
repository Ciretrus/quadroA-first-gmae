using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArtifactObtainReaction : MonoBehaviour
{
    [SerializeField] private Artifact m_artifact;
    [SerializeField] private AudioSource m_burstSound;
    [SerializeField] private AudioSource m_houseSwitchSound;
    [SerializeField] private GameObject m_floor;
    [SerializeField] private GameObject m_ladder;

    [Header("UI elements")]
    [SerializeField] private Image m_blackImage;
    [SerializeField] private TMP_Text m_winText;

    [Header("Timers")]
    [SerializeField] private float m_waitTimer = 5f;
    [SerializeField] private float m_blackScreenTimer = 7f;
    [SerializeField] private float m_textTimer = 3f;

    private CameraMovement m_cameraMovement;
    private Tween m_screenTween;
    private Tween m_textTween;

    private void OnEnable()
    {
        m_artifact.OnTakeArtifact += React;
    }

    private void Start()
    {
        Camera camera = ServiceLocator.Resolve<Camera>();

        m_cameraMovement = camera.GetComponent<CameraMovement>();
    }

    private void OnDisable()
    {
        m_artifact.OnTakeArtifact -= React;
    }

    private void React()
    {
        m_cameraMovement.CameraShake();
        ChangeHouse();
        StartCoroutine(FinishGame());
    }

    private void ChangeHouse()
    {
        m_burstSound.Play();
        m_houseSwitchSound.Play();
        m_floor.SetActive(true);
        m_ladder.SetActive(false);
    }

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(m_waitTimer);
        m_screenTween = AnimationController.Fade(m_screenTween, m_blackImage, 1f, m_blackScreenTimer);
        yield return m_screenTween.WaitForCompletion();

        ServiceLocator.Resolve<ItemsActivations>().ChangeUIMode();

        m_textTween = AnimationController.Fade(m_textTween, m_winText, 1f, m_textTimer);
        yield return m_textTween.WaitForCompletion();

        SceneManager.LoadScene(GlobalConstants.Scenes.MainMenuScene);
    }
}
