using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelParent;
        [SerializeField] private Transform m_playerTransform;

        [SerializeField] private Barrel m_barrel;

        [SerializeField] private float m_fireRate = 1;
        private float m_spawnTimer = 0;

        void Update()
        {
            if (m_barrelParent != null && m_playerTransform != null)
            {
                Quaternion prevRotation = m_barrelParent.rotation;
                m_barrelParent.LookAt(m_playerTransform);
                Vector3 rotation = m_barrelParent.transform.localRotation.eulerAngles;
                if (rotation.x < 270 || rotation.y > 360) m_barrelParent.rotation = prevRotation;

                m_spawnTimer += Time.deltaTime;
                if (m_spawnTimer >= 1 / m_fireRate)
                {
                    m_spawnTimer = 0;
                    m_barrel.Fire();
                }
            }
        }
    }
}
