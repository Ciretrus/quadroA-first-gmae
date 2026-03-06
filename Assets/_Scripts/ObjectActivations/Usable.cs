using UnityEngine;

[RequireComponent(typeof(InteractableSound))]
public abstract class Usable: MonoBehaviour
{
    [SerializeField] protected InteractableSound m_interactableSound;
    public InteractableSound interactableSound => m_interactableSound;

    private UsableType m_type;

    public UsableType type => m_type;

    private void Awake()
    {
        m_interactableSound = GetComponent<InteractableSound>();
    }

    public void Initialize(UsableType type)
    {
        m_type = type;
    }

    private void OnValidate()
    {
        gameObject.TryGetComponent<InteractableSound>(out m_interactableSound);
    }

    public abstract void Use();
}
