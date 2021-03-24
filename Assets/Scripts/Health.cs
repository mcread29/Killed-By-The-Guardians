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

        private void Awake()
        {
            m_health = m_maxHealth;
        }

        public void TakeDamage(int damage)
        {
            m_health = Mathf.Max(m_health - damage);
            if (m_health < 1)
            {
                if (onDeath != null)
                    onDeath();
            }
        }

        public void Heal(int heal)
        {
            m_health = Mathf.Min(m_health + heal);
        }
    }
}
