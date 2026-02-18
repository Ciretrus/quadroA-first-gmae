using UnityEngine;

[RequireComponent(typeof(InteractableSound))]
public abstract class Usable: MonoBehaviour
{
    [SerializeField] protected InteractableSound m_interactableSound;
    public InteractableSound interactableSound => m_interactableSound;

    private void OnEnable()
    {
        m_interactableSound = GetComponent<InteractableSound>();
    }

    private UsableType m_type;

    public UsableType type => m_type;

    public void Initialize(UsableType type)
    {
        m_type = type;
    }

    public abstract void Use();
}
