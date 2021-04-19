using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    [ExecuteInEditMode]
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject m_doorMesh;

        private Room m_room = null;
        public Room room
        {
            get
            {
                if (m_room == null) m_room = GetComponentInParent<Room>();
                return m_room;
            }
        }
        private bool m_attached = false;
        public bool attached { get { return m_attached; } }
        private Door m_attachedDoor = null;

        public System.Action enterDoor;

        public void Close()
        {
            if (m_doorMesh.activeSelf == false)
            {
                m_doorMesh.SetActive(true);
                if (m_attachedDoor != null) m_attachedDoor.Close();
            }
        }

        public void Open()
        {
            if (m_doorMesh.activeSelf == true)
            {
                m_doorMesh.SetActive(false);
                if (m_attachedDoor != null) m_attachedDoor.Open();
            }
        }

        public void Attach(Door door = null)
        {
            m_attached = true;
            m_attachedDoor = door;
            room.SetPreviousDoor(this);
            if (door != null && door.attached == false) door.Attach(this);
        }

        public void Detach()
        {
            m_attached = false;
            if (m_attachedDoor != null && m_attachedDoor.attached) m_attachedDoor.Detach();
            m_attachedDoor = null;
            room.SetPreviousDoor(null);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                if (enterDoor != null) enterDoor();
            }
        }
    }
}
