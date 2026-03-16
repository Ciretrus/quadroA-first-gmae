using UnityEngine;

[RequireComponent(typeof(InteractableSound))]
public abstract class Interactable: MonoBehaviour
{
    [SerializeField] protected InteractableSound m_interactableSound;

    private InteractableType m_type;

    public InteractableSound interactableSound => m_interactableSound;
    public InteractableType type => m_type;

    private void Awake()
    {
        m_interactableSound = GetComponent<InteractableSound>();
    }

    private void OnValidate()
    {
        if (m_interactableSound == null)
        {
            TryGetComponent(out m_interactableSound);
        }
    }

    public void Initialize(InteractableType type)
    {
        m_type = type;
    }

    public abstract void Use();
}
