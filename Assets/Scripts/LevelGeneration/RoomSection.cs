using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSection : MonoBehaviour
    {
        [SerializeField] private Spawnable[] m_enemiesForSection;

        private List<Turret> m_turrets;

        private bool m_spawnedInitial = false;

        public System.Action<RoomSection> sectionComplete;

        private void Awake()
        {
            m_turrets = new List<Turret>();
        }

        public void SetTurretLookAt(Transform transform)
        {
            if (m_enemiesForSection != null)
            {
                foreach (Spawnable s in m_enemiesForSection)
                {
                    if (s != null)
                    {
                        Turret t = s.GetComponent<Turret>();
                        if (t != null)
                        {
                            m_turrets.Add(t);
                            System.Action remove = null;
                            remove = () =>
                            {
                                m_turrets.Remove(t);
                                if (m_turrets.Count <= 0 && sectionComplete != null) sectionComplete(this);
                                t.health.onDeath -= remove;
                            };
                            t.health.onDeath += remove;
                            t.SetTransformLookat(transform);
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            bool generatorCheck = Generator.Instance == null || Generator.Instance.finishedGenerating;
            if (generatorCheck && m_spawnedInitial == false && other.tag == "Player")
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
