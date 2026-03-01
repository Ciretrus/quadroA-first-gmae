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
            Debug.LogError("Cutscene Director is null");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController) && (!m_playOnce || !m_hasPlayed))
        {
            m_cutsceneDirector.Play();
            m_hasPlayed = true;
        }
    }
}
