using System.Collections.Generic;
using UnityEngine;

public class SoundableFloor : MonoBehaviour
{
    [SerializeField] private List<AudioClip> m_floorAudioClips;

    public List<AudioClip> floorAudioClips => m_floorAudioClips;
}
