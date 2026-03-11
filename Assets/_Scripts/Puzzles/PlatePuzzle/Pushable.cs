using UnityEngine;

public class Pushable : MonoBehaviour
{
    [SerializeField] private AudioClip m_audioClip;

    public AudioClip audioClip => m_audioClip;
}
