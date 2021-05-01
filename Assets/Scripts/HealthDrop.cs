using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class HealthDrop : MonoBehaviour
    {
        [SerializeField] private int m_healAmount;
        public int healAmount { get { return m_healAmount; } }

        private void OnTriggerEnter(Collider other)
        {
            Health health = other.GetComponentInParent<Health>();
            Debug.Log($"{other.name} {health}");
            if (health != null && health.canHeal)
            {
                health.Heal(healAmount);
                gameObject.SetActive(false);
            }
        }
    }
}
