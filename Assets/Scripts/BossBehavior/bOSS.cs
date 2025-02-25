using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public abstract class bOSS : MonoBehaviour
    {
        public System.Action onBossKilled;

        protected bool m_bossKilled = false;
        public bool bossKilled { get { return m_bossKilled; } }

        protected Health m_health;

        public abstract void StartBoss();
    }
}
