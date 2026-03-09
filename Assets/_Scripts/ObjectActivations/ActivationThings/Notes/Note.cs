using UnityEngine;

public class Note : Usable
{
    [SerializeField] private NoteData m_noteData;

    private void Awake()
    {
        Initialize(UsableType.Blocking);
    }

    public override void Use()
    {
        ServiceLocator.Resolve<UIController>().ShowNote(m_noteData.sprite);
    }
}
