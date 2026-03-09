using System;
using UnityEngine;

public class TakeHeart : MonoBehaviour
{
    public event Action OnTakeHeart;

    private void OnDestroy()
    {
        OnTakeHeart?.Invoke();
    }
}
