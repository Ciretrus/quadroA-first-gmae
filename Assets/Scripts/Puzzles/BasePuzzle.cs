using System;
using UnityEngine;

namespace Puzzles
{
    abstract public class BasePuzzle : MonoBehaviour
    {
        public event Action onSolved;

        protected bool m_isSolved = false;

        protected void NotifySolved()
        {
            if (m_isSolved) return;

            m_isSolved = true;
            onSolved?.Invoke();
            Debug.Log("solved");
        }

        public abstract void CheckCondition();
    }
}
