using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int m_maxHealth;
        private int m_health;

        public System.Action onDeath;

        [SerializeField] private float m_hitStun = 1;
        private float m_stunTimer = 0;

        private void Awake()
        {
            m_health = m_maxHealth;
        }

        private void Update()
        {
            m_stunTimer -= Time.deltaTime;
        }

        public void TakeDamage(int damage)
        {
            if (m_stunTimer > 0) return;

            m_health = Mathf.Max(m_health - damage);
            Debug.Log(gameObject.name + " TOOK " + damage + " PTS OF DAMAGE AND NOW HAS " + m_health + " HEALTH");
            m_stunTimer = m_hitStun;
            if (m_health < 1)
            {
                if (onDeath != null) onDeath();
            }
        }

        public void Heal(int heal)
        {
            m_health = Mathf.Min(m_health + heal);
        }
    }
}
