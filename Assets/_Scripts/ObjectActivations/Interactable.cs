using UnityEngine;

[RequireComponent(typeof(InteractableSound))]
public abstract class Interactable: MonoBehaviour
{
    [SerializeField] protected InteractableSound m_interactableSound;
    public InteractableSound interactableSound => m_interactableSound;

    private InteractableType m_type;

    public InteractableType type => m_type;

    private void Awake()
    {
        m_interactableSound = GetComponent<InteractableSound>();
    }

    public void Initialize(InteractableType type)
    {
        m_type = type;
    }

    private void OnValidate()
    {
        gameObject.TryGetComponent(out m_interactableSound);
    }

    public abstract void Use();
}
