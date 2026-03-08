using System.Collections;
using UnityEngine;
using DG.Tweening;

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
            transform.DOKill();
        }

        public override void Use()
        {
            if (!m_clicked && m_canClick)
            {
                ClickAnimation();

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

        private void ClickAnimation()
        {
            m_clicked = true;
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOLocalMove(m_newPosition, m_buttonData.timeClick).SetEase(Ease.InQuad));
            s.Append(transform.DOLocalMove(m_startPosition, m_buttonData.timeClick).SetEase(Ease.OutBack));
            s.OnComplete(() => m_clicked = false);
        }

    }
}