using UnityEngine;

public class ArtifactObtainReaction : MonoBehaviour
{
    [SerializeField] private Artifact m_artifact;
    [SerializeField] private AudioSource m_sound;

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
        m_sound.Play();
    }
}
