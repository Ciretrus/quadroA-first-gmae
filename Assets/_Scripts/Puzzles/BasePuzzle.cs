using System;
using UnityEngine;
using UnityEngine.Events;
namespace Puzzles
{
    abstract public class BasePuzzle : MonoBehaviour
    {
        public event Action Solved;
        public UnityEvent SolvedUnityEvent;
        protected bool m_isSolved = false;

        protected void NotifySolved()
        {
            if (m_isSolved) return;

            m_isSolved = true;
            Solved?.Invoke();
            SolvedUnityEvent?.Invoke();
        }

        public abstract void CheckCondition();
    }
}