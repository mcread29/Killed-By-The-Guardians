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

        public void Start()
        {
            foreach (BossDoor door in m_bossDoors)
            {
                Debug.Log(door.name);
                door.enterDoor += startBoss;
            }
            m_boss.bossKilled += FinishRoom;
        }

        public override void FinishRoom()
        {
            base.FinishRoom();

            foreach (BossDoor door in m_bossDoors)
            {
                Debug.Log(door.name);
                door.Open();
            }
        }

        private void startBoss()
        {
            foreach (BossDoor door in m_bossDoors)
            {
                door.Close();
            }
            m_boss.StartBoss();
        }
    }
}
