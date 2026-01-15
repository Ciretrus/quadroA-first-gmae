using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Usable: MonoBehaviour
{
    [SerializeField] protected AudioClip[] m_audioclip;

    public AudioClip[] audioclip => m_audioclip;

    private UsableType m_type;

    public UsableType type => m_type;

    public void Initialize(UsableType type)
    {
        m_type = type;
    }

    public virtual void Use() { }
}
