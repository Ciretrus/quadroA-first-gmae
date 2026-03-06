using System.Runtime.CompilerServices;
using UnityEngine;

public class CrystalBreak : Usable
{
    [SerializeField] private GameObject m_particle;
    public override void Use()
    {
        m_particle.SetActive(true);
        Destroy(m_particle,3f);
        Destroy(gameObject);
    }
}
