using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace UntitledFPS
{
    public class Generator : MonoBehaviour
    {
        private static Generator m_instance;
        public static Generator Instance { get { return m_instance; } }

        private bool m_finishedGenerating = false;
        public bool finishedGenerating { get { return m_finishedGenerating; } }

        [SerializeField] private int m_numAttempts = 10;
        [SerializeField] private LevelData m_data;

        //BROKEN TEST SEED;
        [SerializeField] private bool m_useCustomSeed = false;
        [SerializeField] private int m_randomSeed = -1;

        private List<RoomSceneRoot> m_rooms;

        private Player m_player;

        private int m_numRooms;
        private List<(Room, RoomSceneRoot)> m_roomSceneRoots;
        private List<Door> m_allDoors;

        private void Awake()
        {
            if (m_instance != null)
                Destroy(gameObject);
            else
                m_instance = this;
        }

        private void Start()
        {
            Generate();
        }

        [ContextMenu("GENERATE")]
        public void Generate()
        {
            int numRooms = m_numRooms = Random.Range(m_data.minLength, m_data.maxLength);

            bool successfullGeneration = false;
            int attempts = 0;
            while (successfullGeneration == false && attempts < m_numAttempts)
            {
                if (m_useCustomSeed == false || m_randomSeed == -1) m_randomSeed = new System.Random().Next();
                Random.InitState(m_randomSeed);

                m_rooms = new List<RoomSceneRoot>();
                RoomSceneRoot startRoom = addStartRoom();
                successfullGeneration = newRoom(startRoom, numRooms);

                if (successfullGeneration == false)
                {
                    m_rooms.Remove(startRoom);
                    DestroyImmediate(startRoom.gameObject);
                    Debug.Log($"FAILED ATTEMPT {attempts + 1}");
                    attempts++;
                }
            }

            foreach (RoomSceneRoot root in m_rooms)
            {
                root.DestroyLighting();

                if (root.player != null && m_player == null)
                {
                    m_player = root.player;
                    m_player.gameObject.SetActive(false);
                }
                else if (root.player != null)
                    DestroyImmediate(root.player.gameObject);

                root.SetPlayer(m_player);

                root.room.AttachDoors();

                m_player.gameObject.SetActive(true);
                m_finishedGenerating = true;
            }
            m_finishedGenerating = true;
        }

        private RoomSceneRoot addStartRoom()
        {
            int ind = Random.Range(0, m_data.startRooms.Length);
            RoomSceneRoot startRoom = Instantiate(m_data.startRooms[ind], Vector3.zero, m_data.startRooms[ind].transform.rotation, transform);
            m_rooms.Add(startRoom);
            return startRoom;
        }
        private bool newRoom(RoomSceneRoot previousRoom, int roomCount)
        {
            bool finalRoom = roomCount < 1;
            RoomSceneRoot[] availableRooms = finalRoom ? m_data.endingRooms : m_data.availableRooms;

            List<RoomSceneRoot> untriedRooms = new List<RoomSceneRoot>(availableRooms);

            while (untriedRooms.Count > 0)
            {
                int roomInd = Random.Range(0, untriedRooms.Count);
                RoomSceneRoot nextRoom = Instantiate(availableRooms[roomInd], Vector3.zero, availableRooms[roomInd].transform.rotation, transform);
                untriedRooms.RemoveAt(roomInd);

                //SHOULD CHANGE TO nextRoom.room.name == previousRoom.room.name when we have more in the new system
                bool sameAsPreviousRoom = nextRoom.room.name == previousRoom.room.name;

                if (sameAsPreviousRoom == false)
                {
                    List<Door> untriedNewDoors = new List<Door>(nextRoom.room.doors);
                    while (untriedNewDoors.Count > 0)
                    {
                        int ind = Random.Range(0, untriedNewDoors.Count);
                        Door nextDoor = untriedNewDoors[ind];
                        untriedNewDoors.RemoveAt(ind);

                        List<Door> untriedOldDoors = new List<Door>(previousRoom.room.doors);
                        while (untriedOldDoors.Count > 0)
                        {
                            bool overlap = false;

                            int newInd = Random.Range(0, untriedOldDoors.Count);
                            Door doorToAttach = untriedOldDoors[newInd];
                            untriedOldDoors.RemoveAt(newInd);

                            if (Mathf.Abs(doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y) != 180) continue;

                            alignRoomToDoor2(nextRoom, nextDoor, doorToAttach);

                            foreach (var roomRoot in m_rooms)
                            {
                                bool checkOverlap = nextRoom.room.volume.CheckVolume(roomRoot.room.volume);
                                overlap = overlap || checkOverlap;
                                if (overlap) break;
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
                }

                m_rooms.Remove(nextRoom);
                DestroyImmediate(nextRoom.gameObject);
            }
            return false;
        }
        private void alignRoomToDoor2(RoomSceneRoot nextRoom, Door nextDoor, Door doorToAttach)
        {
            Vector3 nextLocalPos = nextDoor.transform.localPosition;

            nextRoom.transform.Rotate(new Vector3(0, doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y + 180, 0));
            nextLocalPos = Quaternion.Euler(nextRoom.transform.rotation.eulerAngles) * (nextLocalPos - Vector3.zero) + Vector3.zero;
            nextRoom.transform.position = doorToAttach.transform.position - nextLocalPos;
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            if (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
                if (transform.childCount > 0) Clear();
            }
        }
    }
}
