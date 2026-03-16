using UnityEngine;
using System;
using System.Collections;

public class ForestDisable : MonoBehaviour
{
    [SerializeField] private TeleportTrigger m_trigger;
    [SerializeField] private GameObject m_forest;

    private void OnEnable()
    {
        m_trigger.Trigger += ForestOff;
    }

    private void OnDisable()
    {
        m_trigger.Trigger -= ForestOff;
    }

    public void ForestOff(Action action)
    {
        StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(5f);
        m_forest.SetActive(false);
    }
}
