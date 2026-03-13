using UnityEngine;

public class ArtifactObtainReaction : MonoBehaviour
{
    [SerializeField] private Artifact m_artifact;
    [SerializeField] private AudioSource m_burstSound;
    [SerializeField] private AudioSource m_houseSwitchSound;
    [SerializeField] private GameObject m_floor;
    [SerializeField] private GameObject m_ladder;
    

    private CameraMovement m_cameraMovement;

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
        m_burstSound.Play();
        m_houseSwitchSound.Play(); 
        m_floor.SetActive(true);
        m_ladder.SetActive(false);

    }
}
