using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class BossDoor : MonoBehaviour
    {
        [SerializeField] private GameObject m_doorMesh;
        public System.Action enterDoor;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
            if (other.tag == "Player")
            {
                if (enterDoor != null) enterDoor();
            }
        }

        public void Open()
        {
            m_doorMesh.SetActive(false);
        }

        public void Close()
        {
            m_doorMesh.SetActive(true);
        }
    }
}
