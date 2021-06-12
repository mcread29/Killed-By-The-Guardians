using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class BossRoom : Room
    {

        [SerializeField] private BossDoor[] m_bossDoors;
        public BossDoor[] bossDoors { get { return m_bossDoors; } }

        [SerializeField] private bOSS m_boss;

        public static System.Action bossFinished;

        public void Start()
        {
            foreach (BossDoor door in m_bossDoors)
            {
                door.enterDoor += startBoss;
            }
            m_boss.onBossKilled += FinishRoom;
        }

        public override void FinishRoom()
        {
            if (m_boss.bossKilled == false) return;

            base.FinishRoom();
            if (bossFinished != null) bossFinished();
        }

        private void startBoss()
        {
            if (m_boss.bossKilled) return;
            foreach (BossDoor door in m_bossDoors)
            {
                door.Close();
            }
            m_boss.StartBoss();
        }
    }
}
