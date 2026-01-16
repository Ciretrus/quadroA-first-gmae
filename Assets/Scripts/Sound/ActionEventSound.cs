using Puzzles;
using UnityEngine;

public class ActionEventSound : InteractableSound
{
    [SerializeField] private BasePuzzle m_puzzle;

    private void OnEnable()
    {
        if (m_puzzle != null)
        {

        }
    }
}
