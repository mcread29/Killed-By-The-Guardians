using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class UI : MonoBehaviour
    {
        private static UI m_instance;
        public static UI Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new UI();
                }
                return m_instance;
            }
        }

        [SerializeField] private Crosshair m_crosshair;
        [SerializeField] private HealthBar m_healthBar;

        private void Awake()
        {
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
        }

        public void SetHealth(float percent)
        {
            m_healthBar.SetHealth(percent);
        }

        public void SetShields(float percent)
        {
            m_healthBar.SetShields(percent);
        }
    }
}
