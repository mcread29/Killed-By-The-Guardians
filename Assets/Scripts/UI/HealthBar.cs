using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform m_shieldsFill;
        [SerializeField] private RectTransform m_healthFill;
        private RectTransform m_background;
        private float m_maxWidth;

        private void Awake()
        {
            m_background = GetComponent<RectTransform>();
            m_maxWidth = m_background.sizeDelta.x;
        }

        public void SetHealth(float percent)
        {
            m_healthFill.sizeDelta = new Vector2(m_maxWidth * percent, m_healthFill.sizeDelta.y);
        }

        public void SetShields(float percent)
        {
            m_shieldsFill.sizeDelta = new Vector2(m_maxWidth * percent, m_shieldsFill.sizeDelta.y);
        }
    }
}
