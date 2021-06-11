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
            Player player = other.GetComponentInParent<Player>();
            if (player != null)
            {
                player.AddHealth(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
