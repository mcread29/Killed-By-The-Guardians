using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Text m_emenyCountText;
        private int m_enemyCount = 0;

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

        public void AddEnemies(int enemies)
        {
            m_enemyCount += enemies;
            m_emenyCountText.text = m_enemyCount.ToString();
        }

        public void HitEnemy()
        {
            m_crosshair.Hit();
        }

        public void KillEnemy()
        {
            m_enemyCount--;
            m_emenyCountText.text = m_enemyCount.ToString();
        }
    }
}
