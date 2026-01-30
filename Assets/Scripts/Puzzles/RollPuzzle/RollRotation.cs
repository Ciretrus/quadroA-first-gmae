using System.Collections;
using UnityEngine;

namespace Puzzles 
{
    public class RollRotation : MonoBehaviour
    {
        [SerializeField] private BasePuzzle m_puzzle;
        [SerializeField] private float m_angleRotation = 72f;
        [SerializeField] private float m_rotationTime = 0.2f;
        [SerializeField] private int m_rightNmber = 0;

        private IEnumerator m_coroutine;
        private int m_currentNumber = 1;

        public bool isRightNumber
        {
            get { return m_currentNumber == m_rightNmber; }
        }

        public void Rotate(int direction)
        {
            //StopCoroutine(coroutine);
            m_coroutine = GetPosition(direction);
            StartCoroutine(m_coroutine);
        }

        private IEnumerator GetPosition(int direction)
        {
            Coroutine coroutine = StartCoroutine(AnimateRotation(direction));
            yield return coroutine;
            m_currentNumber += direction;
            if (m_currentNumber >= 5) m_currentNumber = 0;
            else if (m_currentNumber <= 0) m_currentNumber = 5;
            print(m_currentNumber);
            m_puzzle.CheckCondition();
        }

        private IEnumerator AnimateRotation(int direction)
        {
            int countIterations = (int)(m_rotationTime / Time.deltaTime);
            float frecuency = m_rotationTime / countIterations;

            for (int i = 0; i < countIterations; i++)
            {
                Vector3 angle = new Vector3(direction * m_angleRotation / countIterations, 0, 0);
                transform.Rotate(angle);
                yield return null;
            }
        }
    }
}