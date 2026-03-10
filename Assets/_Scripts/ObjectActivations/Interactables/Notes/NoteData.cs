using UnityEngine;

[CreateAssetMenu]
public class NoteData: ScriptableObject
{
    [SerializeField] private Sprite m_sprite;

    public Sprite sprite => m_sprite;
}