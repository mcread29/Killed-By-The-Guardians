using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSection : MonoBehaviour
    {
        [SerializeField] private Spawnable[] m_enemiesForSection;

        private bool m_spawnedInitial = false;
        private bool m_started = false;

        public void SetTurretLookAt(Transform transform)
        {
            if (m_enemiesForSection != null)
            {
                foreach (Spawnable s in m_enemiesForSection)
                {
                    if (s != null)
                    {
                        Turret t = s.GetComponent<Turret>();
                        if (t != null) t.SetTransformLookat(transform);
                    }
                }
            }
        }

        public void SetStarted()
        {
            m_started = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_started && m_spawnedInitial == false && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (m_enemiesForSection != null)
                {
                    foreach (Spawnable t in m_enemiesForSection)
                    {
                        if (t != null) t.Spawn();
                    }
                    m_spawnedInitial = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //DISABLE SHIT??
        }
    }
}
