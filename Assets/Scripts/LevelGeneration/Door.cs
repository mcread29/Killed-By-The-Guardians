using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Door : MonoBehaviour
    {
        private Room m_room = null;
        private bool m_attached = false;
        public bool attached { get { return m_attached; } }
        private Door m_attachedDoor = null;

        private void Awake()
        {
            m_room = GetComponentInParent<Room>();
        }

        public void Attach(Door door)
        {
            m_attached = true;
            m_attachedDoor = door;
            if (door.attached == false) door.Attach(this);
        }
    }
}
