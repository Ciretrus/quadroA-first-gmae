using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector m_cutsceneDirector;
    [SerializeField] private bool m_playOnce = true;
    private bool m_hasPlayed = false;    

    private void Start()
    {
        if (m_cutsceneDirector == null)
        {
            Debug.LogError("Братан, ты директора катсцен забыл назначить...");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc;
        if (other.TryGetComponent<PlayerController>(out pc) && (!m_playOnce || !m_hasPlayed))
        {
            m_cutsceneDirector.Play();
            m_hasPlayed = true;
        }
    }
}
