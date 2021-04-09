using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Spawnable : MonoBehaviour
    {
        public void Spawn()
        {
            gameObject.SetActive(true);
        }
    }
}
