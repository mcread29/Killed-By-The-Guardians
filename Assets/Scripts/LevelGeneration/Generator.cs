using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] private LevelData m_data;

        private System.Random m_random;

        private List<Room> m_rooms;

        [ContextMenu("Generate")]
        public void Generate()
        {
            m_rooms = new List<Room>();
            m_random = new System.Random();

            int mumRooms = Random.Range(m_data.minLength, m_data.maxLength);
            Room startRoom = Instantiate(m_data.startRooms[Random.Range(0, m_data.startRooms.Length)], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            m_rooms.Add(startRoom);
            addRoom(startRoom, 15);
        }

        private (Room, Door) selectAndAlignRoom(Door doorToAttach)
        {
            Room nextRoom = Instantiate(m_data.availableRooms[Random.Range(0, m_data.availableRooms.Length)], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            Door nextDoor = nextRoom.ChooseNextDoor();

            Vector3 nextLocalPos = nextDoor.transform.localPosition;

            nextRoom.transform.Rotate(new Vector3(0, doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y + 180, 0));
            nextLocalPos = Quaternion.Euler(nextRoom.transform.rotation.eulerAngles) * (nextLocalPos - Vector3.zero) + Vector3.zero;
            nextRoom.transform.position = doorToAttach.transform.position - nextLocalPos;

            return (nextRoom, nextDoor);
        }

        private void addRoom(Room previousRoom, int numberOfRooms)
        {
            Door doorToAttach = previousRoom.pickDoor(m_data.straightness, m_random);

            Room nextRoom = null;
            Door nextDoor = null;
            bool validRoom = false;
            while (validRoom == false)
            {
                (nextRoom, nextDoor) = selectAndAlignRoom(doorToAttach);
                foreach (var room in m_rooms)
                {
                    validRoom = nextRoom.CheckRoomOverlap(room) == false;
                    if (validRoom == false)
                    {
                        DestroyImmediate(nextRoom.gameObject);
                        break;
                    };
                }
            }

            // Room nextRoom = Instantiate(m_data.availableRooms[Random.Range(0, m_data.availableRooms.Length)], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            // Door nextDoor = nextRoom.ChooseNextDoor();

            // Vector3 nextLocalPos = nextDoor.transform.localPosition;

            // nextRoom.transform.Rotate(new Vector3(0, doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y + 180, 0));
            // nextLocalPos = Quaternion.Euler(nextRoom.transform.rotation.eulerAngles) * (nextLocalPos - Vector3.zero) + Vector3.zero;
            // nextRoom.transform.position = doorToAttach.transform.position - nextLocalPos;

            //CHECK FOR OVERLAP WITH OTHER ROOMS


            //FINALIZE ROOM
            doorToAttach.Attach(nextDoor);
            nextDoor.gameObject.SetActive(false);
            doorToAttach.gameObject.SetActive(false);

            m_rooms.Add(nextRoom);

            numberOfRooms--;
            if (numberOfRooms > 0) addRoom(nextRoom, numberOfRooms);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            if (transform.childCount > 0)
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
                if (transform.childCount > 0) Clear();
            }
        }
    }
}
