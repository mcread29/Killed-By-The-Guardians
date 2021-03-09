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
            newRoom(startRoom, 20);
        }

        private void alignRoomToDoor(Room nextRoom, Door nextDoor, Door doorToAttach)
        {
            Vector3 nextLocalPos = nextDoor.transform.localPosition;

            nextRoom.transform.Rotate(new Vector3(0, doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y + 180, 0));
            nextLocalPos = Quaternion.Euler(nextRoom.transform.rotation.eulerAngles) * (nextLocalPos - Vector3.zero) + Vector3.zero;
            nextRoom.transform.position = doorToAttach.transform.position - nextLocalPos;
        }

        private bool newRoom(Room previousRoom, int roomCount)
        {
            Door doorToAttach = null;
            Room nextRoom = null;
            Door nextDoor = null;
            List<Room> untriedRooms = new List<Room>(m_data.availableRooms);

            do
            {
                int ind = m_random.Next(untriedRooms.Count);
                nextRoom = Instantiate(m_data.availableRooms[ind], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
                untriedRooms.RemoveAt(ind);

                List<Door> untriedNewDoors = new List<Door>(nextRoom.doors);
                while (untriedNewDoors.Count > 0)
                {
                    ind = m_random.Next(untriedNewDoors.Count);
                    nextDoor = untriedNewDoors[ind];
                    untriedNewDoors.RemoveAt(ind);

                    List<Door> untriedOldDoors = new List<Door>(previousRoom.doors);
                    while (untriedOldDoors.Count > 0)
                    {
                        bool overlap = false;

                        ind = m_random.Next(untriedOldDoors.Count);
                        doorToAttach = untriedOldDoors[ind];
                        untriedOldDoors.RemoveAt(ind);

                        alignRoomToDoor(nextRoom, nextDoor, doorToAttach);

                        foreach (var room in m_rooms)
                        {
                            overlap = overlap || nextRoom.volume.CheckVolume(room.volume);
                            if (overlap)
                            {
                                Debug.Log("OVERLAP");
                                break;
                            }
                        }

                        if (overlap) continue;
                        else
                        {
                            m_rooms.Add(nextRoom);
                            bool success = roomCount < 1 || newRoom(nextRoom, roomCount - 1);
                            if (success)
                            {
                                doorToAttach.Attach(nextDoor);
                                return true;
                            }
                        }
                    }

                }

                m_rooms.Remove(nextRoom);
                DestroyImmediate(nextRoom.gameObject);
            } while (untriedRooms.Count > 0);
            return false;
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
