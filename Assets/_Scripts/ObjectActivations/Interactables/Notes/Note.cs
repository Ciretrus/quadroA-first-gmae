using UnityEngine;

public class Note : Interactable
{
    [SerializeField] private NoteData m_noteData;

    private void Awake()
    {
        Initialize(InteractableType.Blocking);
    }

    public override void Use()
    {
        ServiceLocator.Resolve<UIController>().ShowNote(m_noteData.sprite);
    }
}
