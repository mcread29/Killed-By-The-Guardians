using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [ExecuteInEditMode]
    public class Room : MonoBehaviour
    {
        [SerializeField] private Door[] m_doors;
        public Door[] doors { get { return m_doors; } }

        private Door m_previousAttached;
        private RoomVolume m_volume;
        public RoomVolume volume { get { return m_volume; } }

        private void Awake()
        {
            m_volume = GetComponent<RoomVolume>();
        }

        public void SetPreviousDoor(Door previous)
        {
            m_previousAttached = previous;
        }

        public bool CheckRoomOverlap(Room room)
        {
            Debug.Log(gameObject.name + ", " + room.gameObject.name);
            Debug.Log(m_volume.count + ", " + room.volume.count);
            for (int i = 0; i < m_volume.count; i++)
            {
                for (int j = 0; j < room.volume.count; j++)
                {
                    Debug.Log("\t" + i + m_volume[i] + ", " + j + room.volume[j]);
                    if (m_volume[i] == room.volume[j]) return true;
                }
            }
            return false;
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
