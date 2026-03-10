using System;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public event Action OnTakeArtifact;

    private void OnDestroy()
    {
        OnTakeArtifact?.Invoke();
    }
}
