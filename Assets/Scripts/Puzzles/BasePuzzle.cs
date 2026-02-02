using System;
using UnityEngine;
using UnityEngine.Events;
namespace Puzzles
{
    abstract public class BasePuzzle : MonoBehaviour
    {
        public event Action m_onSolved;
        public UnityEvent m_onSolvedUnityEvent;
        protected bool m_isSolved = false;

        protected void NotifySolved()
        {
            if (m_isSolved) return;

            m_isSolved = true;
            m_onSolved?.Invoke();
            m_onSolvedUnityEvent?.Invoke();
            Debug.Log("solved");
        }
        public abstract void CheckCondition();
    }

}
