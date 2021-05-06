using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSection : MonoBehaviour
    {
        [SerializeField] private float m_spawnTimer = -1f;

        [SerializeField] private Spawnable[] m_enemiesForSection;

        private List<Turret> m_turrets;
        public int TurretCount { get { return m_turrets != null ? m_turrets.Count : 0; } }

        private bool m_spawnedInitial = false;

        public System.Action<RoomSection> sectionComplete;
        public System.Action sectionStarted;

        public void SetTurretLookAt(Transform transform)
        {
            m_turrets = new List<Turret>();
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
                                UI.Instance.KillEnemy();
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

        public IEnumerator startSectionTimer()
        {
            if (m_spawnTimer < 0) yield break;

            yield return new WaitForSeconds(m_spawnTimer);
            if (m_spawnedInitial == false)
                startSection();
        }

        private void OnTriggerEnter(Collider other)
        {
            bool generatorCheck = Generator.Instance == null || Generator.Instance.finishedGenerating;
            if (generatorCheck && m_spawnedInitial == false && other.tag == "Player")
            {
                startSection();
                if (sectionStarted != null) sectionStarted();
            }
        }

        private void startSection()
        {
            if (m_enemiesForSection != null)
            {
                foreach (Spawnable t in m_enemiesForSection)
                {
                    if (t != null) t.Spawn();
                }
                UI.Instance.AddEnemies(m_turrets.Count);
                m_spawnedInitial = true;
            }
        }

        public void StopShooting()
        {
            foreach (Spawnable t in m_enemiesForSection)
            {
                if (t != null)
                {
                    StaticTurret staticTurret = t.GetComponent<StaticTurret>();
                    if (staticTurret != null) staticTurret.StopFiring();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            //DISABLE SHIT??
        }
    }
}
