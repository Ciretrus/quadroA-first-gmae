using UnityEngine;

public class HeartTakeReaction : MonoBehaviour
{
    [SerializeField] private CameraMovement m_cameraMovement;
    [SerializeField] private TakeHeart m_takeHeart;
    [SerializeField] private AudioSource m_sound;

    private void Awake()
    {
        m_takeHeart.OnTakeHeart += Reaction;
    }
    private void OnDisable()
    {
        m_takeHeart.OnTakeHeart -= Reaction;
    }
    public void Reaction()
    {
        m_cameraMovement.CameraShake();
        m_sound.Play();

    }

    
}
