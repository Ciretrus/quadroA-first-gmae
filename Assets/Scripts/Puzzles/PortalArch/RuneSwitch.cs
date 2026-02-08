using Puzzles;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Puzzles{
    public class RuneSwitch : MonoBehaviour, ICondition
    {

        private MaterialPropertyBlock m_emissionMat;
        private MeshRenderer m_meshRenderer;
        private bool m_isStarted = false;
        private bool m_isActive = false;
        private Tween m_currentTween;

        private Color m_tempColor;

        [SerializeField] private float m_glowTime = 1.0f;
        [SerializeField] private Color m_colorOff = Color.black;
        [SerializeField] private Color m_colorOn = Color.cyan;
        [SerializeField] private int m_materialIndex = 1;

        
        public bool IsSolved
        {
            get { return m_isActive; }
        }
        void Awake()
        {
            m_meshRenderer = GetComponent<MeshRenderer>();
            m_emissionMat = new MaterialPropertyBlock();
        }

        public bool changeState()
        {
            m_isActive = !m_isActive;

            if (m_currentTween != null)
            {
                m_currentTween.Kill();
            }

            Color targetColor;

            if (m_isActive == true) { targetColor = m_colorOn; }
            else { targetColor = m_colorOff; }
            

            m_meshRenderer.GetPropertyBlock(m_emissionMat, m_materialIndex);
            m_tempColor = m_emissionMat.GetColor("_EmissionColor");


            m_currentTween = DOTween.To(GetColor, SetNewColor, targetColor, m_glowTime);

            m_currentTween.SetEase(Ease.InOutQuad);

            return true;

        }


        private Color GetColor()
        {
            return m_tempColor;
        }

        private void SetNewColor(Color val)
        {
            m_tempColor = val;
            m_emissionMat.SetColor("_EmissionColor", m_tempColor);
            m_meshRenderer.SetPropertyBlock(m_emissionMat, m_materialIndex);
        }
    }
}
