using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Health : MonoBehaviour
    {
        // [SerializeField] private int m_maxShields;
        // private int m_shields;
        // [SerializeField] private float m_rechargeTime;
        // [SerializeField] private float m_rechargeDelay;

        [SerializeField] private int m_maxHealth;
        private int m_health;

        public System.Action onDeath;
        public System.Action<float, float> updateHealth;
        // public System.Action<float, float> updateShileds;

        [SerializeField] private float m_hitStun = 1;
        private float m_stunTimer = 0;

        private Shields m_shields;
        public Shields shields { get { return m_shields; } }

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
            if (m_stunTimer > 0) return;

            // StopCoroutine(rechargeShields());
            // StartCoroutine(rechargeShields());

            if (m_shields != null)
            {
                if (m_shields.shields > 0) damage = m_shields.HitShields(damage);
                else m_shields.RestartRecharge();
            }
            // if (m_shields > 0)
            // {
            //     int shieldDamag = Mathf.Min(m_shields, damage);
            //     damage = damage - shieldDamag;
            //     m_shields -= shieldDamag;
            // }

            if (damage > 0)
            {
                m_health = Mathf.Max(m_health - damage);
                // Debug.Log(gameObject.name + " TOOK " + damage + " PTS OF DAMAGE AND NOW HAS " + m_health + " HEALTH");
                m_stunTimer = m_hitStun;
                if (m_health < 1) if (onDeath != null) onDeath();
            }

            healthChanged();
        }

        public void Heal(int heal)
        {
            m_health = Mathf.Min(m_health + heal);
            healthChanged();
        }

        // private IEnumerator rechargeShields()
        // {
        //     yield return new WaitForSeconds(m_rechargeDelay);
        //     while (m_shields < m_maxShields)
        //     {
        //         m_shields += 1;
        //         yield return null;
        //     }
        // }

        private void healthChanged()
        {
            if (updateHealth != null) updateHealth(m_health, m_maxHealth);
        }

        // private void shieldsChanged()
        // {
        //     if (updateShileds != null) updateShileds(m_shields, m_maxShields);
        // }
    }
}
