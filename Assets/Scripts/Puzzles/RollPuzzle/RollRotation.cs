using System.Collections;
using UnityEngine;
namespace Puzzles {
    public class RollRotation : MonoBehaviour
    {
        [SerializeField] private float m_angleRotation = 72f;
        [SerializeField] private float m_rotationTime = 0.2f;
        [SerializeField] private BasePuzzle m_puzzle;

        private int m_currentNumber = 1;
        [SerializeField] private int m_rightNmber = 0;

        public bool isRightNumber
        {
            get { return m_currentNumber == m_rightNmber; }
        }

        private IEnumerator m_coroutine;
        private bool m_finishCoroutine = true;

        public void Rotate(int direction)
        {
            //StopCoroutine(coroutine);
            m_coroutine = getPosition(direction);
            StartCoroutine(m_coroutine);

        }

        IEnumerator getPosition(int direction)
        {
            Coroutine coroutine = StartCoroutine(rotate(direction));
            yield return coroutine;
            m_currentNumber += direction;
            if (m_currentNumber >= 5) m_currentNumber = 0;
            else if (m_currentNumber <= 0) m_currentNumber = 5;
            print(m_currentNumber);
            m_puzzle.CheckCondition();
        }

        IEnumerator rotate(int direction)
        {
            m_finishCoroutine = false;
            int countIterations = (int)(m_rotationTime / Time.deltaTime);
            float frecuency = m_rotationTime / countIterations;

            for (int i = 0; i < countIterations; i++)
            {
                Vector3 angle = new Vector3(direction * m_angleRotation / countIterations, 0, 0);
                transform.Rotate(angle);
                yield return null;
            }

            m_finishCoroutine = true;
        }
    }
}