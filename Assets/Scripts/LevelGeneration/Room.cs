using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [ExecuteInEditMode]
    public class Room : MonoBehaviour
    {
        [SerializeField] private string m_roomName;
        public string roomName { get { return m_roomName; } }

        [SerializeField] private bool m_startRoom;
        [SerializeField] private bool m_endRoom;

        [SerializeField] private Door[] m_doors;
        public Door[] doors { get { return m_doors; } }

        private bool m_roomFinished = false;

        private Door m_previousAttached;
        private RoomVolume m_volume;
        public RoomVolume volume { get { return m_volume; } }

        private void Awake()
        {
            m_volume = GetComponent<RoomVolume>();
        }

        public virtual void AttachDoors()
        {
            foreach (Door door in m_doors)
            {
                door.enterDoor += enterDoor;
                if (m_startRoom) door.Open();
            }
        }

        private void enterDoor()
        {
            if (m_startRoom == false && m_roomFinished == false)
            {
                foreach (Door door in m_doors)
                {
                    door.Close();
                }
            }
        }

        public virtual void FinishRoom()
        {
            m_roomFinished = true;
            foreach (Door door in m_doors)
            {
                if (door.attached && door.attachedDoor.room.m_roomFinished == false && door.attachedDoor.room.m_startRoom == false)
                    door.Open();
            }
        }

        public void SetPreviousDoor(Door previous)
        {
            m_previousAttached = previous;
        }

        public Door ChooseNextDoor()
        {
            Door selectedDoor = null;
            List<Door> unTried = new List<Door>(m_doors);
            System.Random r = new System.Random();
            while (unTried.Count > 0 && selectedDoor == null)
            {
                int ind = r.Next(unTried.Count);
                Door d = unTried[ind];
                unTried.RemoveAt(ind);
                if (d.attached == false) selectedDoor = d;
            }
            return selectedDoor;
        }

        public Door pickDoor(float straightness, System.Random random)
        {
            if (doors.Length < 2) return doors[0];

            int angleToMatch = Random.Range(0f, 1f) > straightness ? 90 : 180;

            Door selectedDoor = null;
            List<Door> unTried = new List<Door>(doors);
            while (unTried.Count > 0 && selectedDoor == null)
            {
                int ind = random.Next(unTried.Count);
                Door d = unTried[ind];
                unTried.RemoveAt(ind);
                if (d.attached == false && (int)Mathf.Abs(m_previousAttached.transform.eulerAngles.y - d.transform.eulerAngles.y) % angleToMatch == 0)
                    selectedDoor = d;
            }

            if (selectedDoor == null)
            {
                foreach (var door in doors)
                {
                    if (door.attached == false)
                    {
                        selectedDoor = door;
                        break;
                    }
                }
            }
            return selectedDoor;
        }
    }
}
