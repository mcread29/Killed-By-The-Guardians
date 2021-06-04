using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [RequireComponent(typeof(Health))]
    public class Shields : MonoBehaviour
    {
        [SerializeField] private int m_maxShields;
        private int m_shields;
        public int shields { get { return m_shields; } }
        [SerializeField] private float m_rechargeTime;
        [SerializeField] private float m_rechargeDelay;
        [SerializeField] private HealthBar m_healthBar;

        public System.Action<float, float> updateShileds;

        private Coroutine recharging;

        private void Awake()
        {
            m_shields = m_maxShields;
        }

        public int HitShields(int damage)
        {
            RestartRecharge();

            int remainingDamage = Mathf.Max(damage - m_shields, 0);
            m_shields -= damage;
            shieldsChanged();
            return remainingDamage;
        }

        public void RestartRecharge()
        {
            if (recharging != null) StopCoroutine(recharging);
            recharging = StartCoroutine(rechargeShields());
        }

        private IEnumerator rechargeShields()
        {
            yield return new WaitForSeconds(m_rechargeDelay);

            int shieldsToRecharge = m_maxShields - m_shields;
            float percentToRecharge = shieldsToRecharge / m_maxShields;
            float adjustedRechargeTime = percentToRecharge * m_rechargeTime;
            float recharge1Time = adjustedRechargeTime / shieldsToRecharge;

            while (m_shields < m_maxShields)
            {
                yield return new WaitForSeconds(recharge1Time);
                m_shields += 1;
                shieldsChanged();
            }
            Debug.Log(m_shields + ", " + m_maxShields);
        }

        private void shieldsChanged()
        {
            if (updateShileds != null) updateShileds(m_shields, m_maxShields);
            if (m_healthBar != null) m_healthBar.SetShields((float)m_shields / (float)m_maxShields);
        }
    }
}
