using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSection : MonoBehaviour
    {
        [SerializeField] private float m_spawnTimer = -1f;

        [SerializeField] private bool m_bossRoom = false;

        [SerializeField] private Spawnable[] m_enemiesForSection;

        private List<Turret> m_turrets;
        private int m_turretCount = 0;
        public int TurretCount { get { return m_turretCount; } }

        private bool m_spawnedInitial = false;

        public System.Action<RoomSection> sectionComplete;
        public System.Action sectionStarted;

        public void SetTurretLookAt(Player player)
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

                            System.Action<float, float> hit = null;
                            hit = (float h, float mh) => UI.Instance.HitEnemy();
                            t.health.updateHealth += hit;

                            System.Action remove = null;
                            remove = () =>
                            {
                                m_turretCount--;
                                UI.Instance.KillEnemy();
                                if (m_turretCount <= 0 && sectionComplete != null) sectionComplete(this);
                                if (m_bossRoom == false)
                                {
                                    t.health.onDeath -= player.Kill;
                                    t.health.onDeath -= remove;
                                    t.health.updateHealth -= hit;
                                }
                            };
                            t.health.onDeath += player.Kill;
                            t.health.onDeath += remove;

                            t.SetTransformLookat(player.transform);
                        }
                    }
                }
            }
            m_turretCount = m_turrets.Count;
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
            StartCoroutine(WaitToSpawn(other.tag));
        }

        private IEnumerator WaitToSpawn(string tag)
        {
            bool generatorCheck = Generator.Instance == null || Generator.Instance.finishedGenerating;
            if (generatorCheck && m_spawnedInitial == false && tag == "Player")
            {
                while (UI.Instance == null || m_turrets == null)
                    yield return new WaitForEndOfFrame();
                startSection();
                if (sectionStarted != null) sectionStarted();
            }
        }

        public void startSection()
        {
            if (m_enemiesForSection != null)
            {
                foreach (Spawnable t in m_enemiesForSection)
                {
                    if (t != null) t.Spawn();
                }
                m_turretCount = m_turrets.Count;
                UI.Instance.AddEnemies(m_turretCount);
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
