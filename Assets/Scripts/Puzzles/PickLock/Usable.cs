using UnityEngine;

public abstract class Usable: MonoBehaviour
{
    private UsableType m_type;

    public UsableType type => m_type;

    public void Initialize(UsableType type)
    {
        m_type = type;
    }

    public virtual void Use() { }
}
