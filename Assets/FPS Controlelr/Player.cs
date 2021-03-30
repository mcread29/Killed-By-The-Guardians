using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Player : MonoBehaviour
    {
        private FPSController.PlayerMovement m_movement;
        private Health m_health;

        private void Awake()
        {
            m_movement = GetComponent<FPSController.PlayerMovement>();
            m_health = GetComponent<Health>();
        }

        private void Start()
        {
            m_health.updateHealth += changeHealth;
            if (m_health.shields) m_health.shields.updateShileds += changeShields;
        }

        private void OnDestroy()
        {
            m_health.updateHealth -= changeHealth;
        }

        private void changeHealth(float newHealth, float maxHealth)
        {
            UI.Instance.SetHealth(newHealth / maxHealth);
        }

        private void changeShields(float newShields, float maxShields)
        {
            UI.Instance.SetShields(newShields / maxShields);
        }
    }
}
