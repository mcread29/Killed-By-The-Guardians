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
            Go.killAllTweensWithTarget(m_healthFill);
            GoTweenConfig config = new GoTweenConfig();
            config.sizeDelta(new Vector2(m_maxWidth * percent, m_healthFill.sizeDelta.y));
            config.setEaseType(GoEaseType.QuadInOut);
            Go.to(m_healthFill, 0.12f, config);
        }

        public void SetShields(float percent)
        {
            Go.killAllTweensWithTarget(m_shieldsFill);
            GoTweenConfig config = new GoTweenConfig();
            config.sizeDelta(new Vector2(m_maxWidth * percent, m_shieldsFill.sizeDelta.y));
            config.setEaseType(GoEaseType.QuadInOut);
            Go.to(m_shieldsFill, 0.12f, config);
        }
    }
}
