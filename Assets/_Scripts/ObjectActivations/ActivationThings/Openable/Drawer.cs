using UnityEngine;

public class Drawer : Openable
{
    [SerializeField] private AudioClip[] m_openSounds;
    [SerializeField] private AudioClip[] m_closeSounds;
    public override void Use()
    {
        if (IsOpened)
            m_interactableSound.ChangeSounds(m_closeSounds);
        else
            m_interactableSound.ChangeSounds(m_openSounds);
        Open(); 
    }
}
