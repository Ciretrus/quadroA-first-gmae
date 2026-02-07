using System;
using UnityEngine;
using UnityEngine.Events;
namespace Puzzles
{
    abstract public class BasePuzzle : MonoBehaviour
    {
        public event Action onSolved;
        public UnityEvent m_onSolvedUnityEvent;
        protected bool m_isSolved = false;

        protected void NotifySolved()
        {
            if (m_isSolved) return;

            m_isSolved = true;
            onSolved?.Invoke();
            m_onSolvedUnityEvent?.Invoke();
            Debug.Log("solved");
        }
        public abstract void CheckCondition();
    }

}
