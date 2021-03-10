using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSceneRoot : MonoBehaviour
    {
        [SerializeField] private GameObject m_lighting;

        private void Awake()
        {
            Destroy(m_lighting.gameObject);
        }
    }
}
