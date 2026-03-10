using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class DrawingData : ScriptableObject
{
    [SerializeField] private string m_name;
    [SerializeField] private List<Vector3> m_original;

    [NonSerialized] private bool m_solved = false;

    public string runeName { get { return m_name; } }
    public List<Vector3> original { get { return m_original; } }
    public bool solved { get { return m_solved; } set { m_solved = value; } }
}