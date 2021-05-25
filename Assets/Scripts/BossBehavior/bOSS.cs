using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public abstract class bOSS : MonoBehaviour
    {
        public System.Action bossKilled;
        public abstract void StartBoss();
    }
}
