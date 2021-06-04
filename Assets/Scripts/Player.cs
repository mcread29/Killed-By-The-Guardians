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
            MoveToLayer(transform, gameObject.layer);
        }

        private void Start()
        {
            m_health.onDeath += Death;
        }

        private void Death()
        {
            m_health.onDeath -= Death;

            m_movement.Lock();
            UI.Instance.PlayerKilled();
        }

        private void OnDestroy()
        {
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }
    }
}
