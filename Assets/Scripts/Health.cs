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
        public System.Action<float, float> updateHealth;

        [SerializeField] private float m_hitStun = 1;
        private float m_stunTimer = 0;

        [SerializeField] private bool m_canHeal = false;
        public bool canHeal { get { return m_canHeal; } }

        private Shields m_shields;
        public Shields shields { get { return m_shields; } }

        public bool dead { get { return m_health <= 0; } }

        private void Awake()
        {
            m_health = m_maxHealth;
            m_shields = GetComponent<Shields>();
        }

        private void Update()
        {
            m_stunTimer -= Time.deltaTime;
        }

        public void TakeDamage(int damage)
        {
            if (m_stunTimer > 0 || enabled == false) return;

            if (m_shields != null)
            {
                if (m_shields.shields > 0) damage = m_shields.HitShields(damage);
                else m_shields.RestartRecharge();
            }

            if (damage > 0)
            {
                m_health = Mathf.Max(m_health - damage);
                m_stunTimer = m_hitStun;
                if (m_health < 1)
                    if (onDeath != null) onDeath();
            }

            healthChanged();
        }

        public void Heal(int heal)
        {
            m_health = Mathf.Min(m_health + heal, m_maxHealth);
            healthChanged();
        }

        public void FullHeal()
        {
            Heal(m_maxHealth);
        }

        private void healthChanged()
        {
            if (updateHealth != null) updateHealth(m_health, m_maxHealth);
        }

        private void OnTriggerEnter(Collider other)
        {

        }
    }
}
