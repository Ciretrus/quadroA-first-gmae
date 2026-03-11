using UnityEngine;

[CreateAssetMenu]
public class DiaryInteractableData : ScriptableObject
{
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private string m_filename;

    public Sprite sprite => m_sprite;
    public string filename => m_filename;
}
