using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Collider))]
public class Door : Openable
{
    [SerializeField] private Collider m_collider;

    public override void Use()
    {
        m_collider.enabled = false;

        Open();

        StartCoroutine(EnableCollision());
    }

    private IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(m_timer);
        m_collider.enabled = true;
    }
}
