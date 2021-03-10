using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class RoomSceneRoot : MonoBehaviour
    {
        [SerializeField] private GameObject m_lighting;
        [SerializeField] private Room m_room;
        public Room room { get { return m_room; } }

        private void Awake()
        {
            Destroy(m_lighting.gameObject);
        }
    }
}
