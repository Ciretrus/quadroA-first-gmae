using System.Collections;
using UnityEngine;

namespace Puzzles.PortalArch
{
    public class ButtonsClick : Usable
    {
        [SerializeField] private ButtonData m_buttonData;
        [SerializeField] private BasePuzzle m_prerequisitePuzzle;
        [SerializeField] private BasePuzzle m_puzzle;
        [SerializeField] private RuneSwitch[] m_activatedRunes;

        private bool m_canClick = false;
        private bool m_clicked = false;
        private Vector3 m_startPosition;
        private Vector3 m_newPosition;

        private void Awake()
        {
            Initialize(UsableType.NonBlocking);

            m_startPosition = transform.localPosition;
            m_newPosition = m_startPosition - m_buttonData.clickedShiftPosition;
        }

        public void OnEnable()
        {
            m_prerequisitePuzzle.Solved += EnableButtons;
        }

        private void OnDisable()
        {
            m_prerequisitePuzzle.Solved -= EnableButtons;
        }

        public override void Use()
        {
            if (!m_clicked && m_canClick)
            {
                StartCoroutine(Click());

                for (int i = 0; i < m_activatedRunes.Length; i++)
                {
                    m_activatedRunes[i].ChangeState();
                }

                m_puzzle.CheckCondition();
            }
        }

        private void EnableButtons()
        {
            m_canClick = true;
        }

        private IEnumerator Click()
        {
            m_clicked = true;
            Coroutine coroutine = StartCoroutine(ChangePosition(m_newPosition));
            yield return coroutine;
            coroutine = StartCoroutine(ChangePosition(m_startPosition));
            yield return coroutine;
            m_clicked = false;
        }

        private IEnumerator ChangePosition(Vector3 newPosition)
        {
            float t = 0;
            Vector3 startPosition = transform.localPosition;

            while (t < 1)
            {
                t += Time.deltaTime / m_buttonData.timeClick;
                transform.localPosition = Vector3.Lerp(startPosition, newPosition, t);
                yield return null;
            }
        }
    }
}