using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [ExecuteInEditMode]
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
            gameObject.SetActive(false);
            m_room.SetPreviousDoor(this);
        }

        public void Detach()
        {
            m_attached = false;
            if (m_attachedDoor.attached) m_attachedDoor.Detach();
            m_attachedDoor = null;
            gameObject.SetActive(true);
            m_room.SetPreviousDoor(null);
        }
    }
}
