using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSection : MonoBehaviour
    {
        [SerializeField] private Turret[] m_enemiesForSection;
        public Turret[] enemiesForSection { get { return m_enemiesForSection; } }

        private bool m_playerSet = false;
        private bool m_started = false;

        public void SetPlayer(Player player)
        {
            foreach (Turret t in m_enemiesForSection)
            {
                if (t != null) t.SetLookat(player.transform);
            }
            m_playerSet = true;
        }

        private void Start()
        {
            m_started = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(m_started + ", " + m_playerSet);
            if (m_started && m_playerSet && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (m_enemiesForSection != null)
                {
                    foreach (Turret t in m_enemiesForSection)
                    {
                        if (t != null) t.Spawn();
                    }
                }
            }
        }
    }
}
