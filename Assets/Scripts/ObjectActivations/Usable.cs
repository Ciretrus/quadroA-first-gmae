using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Usable: MonoBehaviour
{
    [SerializeField] protected InteractableSound m_interactableSound;
    public InteractableSound interactableSound => m_interactableSound;

    private UsableType m_type;

    public UsableType type => m_type;

    public void Initialize(UsableType type)
    {
        m_type = type;
    }

    public virtual void Use() { }
}
