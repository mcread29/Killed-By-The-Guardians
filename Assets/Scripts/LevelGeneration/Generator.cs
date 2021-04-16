using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

namespace UntitledFPS
{
    //MUST MATCH NAME OF ROOM SCENE
    public enum RoomName
    {
        StartRoom,
        Room1,
        Room2,
        Room3,
        Room4,
        Room5,
        Room6,
        BossRoom1,
        TestRoom,
        RoughRoom1
    }

    public class Generator : MonoBehaviour
    {
        private static Generator m_instance;
        public static Generator Instance { get { return m_instance; } }

        private bool m_finishedGenerating = false;
        public bool finishedGenerating { get { return m_finishedGenerating; } }

        [SerializeField] private int m_numAttempts = 10;
        [SerializeField] private LevelData m_data;

        private System.Random m_random;
        private List<Room> m_rooms;

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

        [ContextMenu("Generate")]
        public void Generate()
        {
            m_random = new System.Random();

            int numRooms = m_numRooms = Random.Range(m_data.minLength, m_data.maxLength);

            bool successfullGeneration = false;
            int attempts = 0;
            while (successfullGeneration == false && attempts < m_numAttempts)
            {
                m_rooms = new List<Room>();
                Room startRoom = addStartRoom();
                successfullGeneration = newRoom(startRoom, numRooms);

                if (successfullGeneration == false)
                {
                    GameObject attempt = new GameObject("ATTEMPT " + attempts);

                    List<Transform> children = new List<Transform>();
                    for (int i = 0; i < transform.childCount; i++)
                        children.Add(transform.GetChild(i));
                    foreach (Transform t in children)
                        t.SetParent(attempt.transform);

                    attempt.SetActive(false);
                    attempts++;
                }
            }

            m_roomSceneRoots = new List<(Room, RoomSceneRoot)>();
            m_allDoors = new List<Door>();

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                loadNextScene();
            }
#else
            loadNextScene();
#endif
        }

        private Room addStartRoom()
        {
            int ind = Random.Range(0, m_data.startRooms.Length);
            Room startRoom = Instantiate(m_data.startRooms[ind], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            m_rooms.Add(startRoom);
            return startRoom;
        }

        private void loadNextScene()
        {
            Room room = m_rooms[0];
            m_rooms[0].gameObject.SetActive(false);
            m_rooms.RemoveAt(0);

            string roomName = room.roomName;
            SceneManager.LoadScene(roomName, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += roomSceneLoaded(room, roomName);
        }

        private UnityAction<Scene, LoadSceneMode> roomSceneLoaded(Room room, string sceneName)
        {
            UnityAction<Scene, LoadSceneMode> a = null;
            a = (Scene scene, LoadSceneMode l) =>
            {
                if (scene.name == sceneName)
                {
                    StartCoroutine(moveAfterLoad(scene, room));
                    SceneManager.sceneLoaded -= a;
                }
            };
            return a;
        }

        private IEnumerator moveAfterLoad(Scene scene, Room room)
        {
            while (scene.isLoaded == false) yield return new WaitForEndOfFrame();

            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                rootGameObject.transform.position = room.gameObject.transform.position;
                RoomSceneRoot root = rootGameObject.GetComponent<RoomSceneRoot>();

                if (root != null)
                {
                    root.DestroyLighting();

                    if (root.player != null && m_player == null)
                    {
                        m_player = root.player;
                        m_player.gameObject.SetActive(false);
                    }
                    else if (root.player != null)
                        Destroy(root.player.gameObject);

                    for (int i = 0; i < room.doors.Length; i++)
                    {
                        m_allDoors.Add(root.room.doors[i]);
                    }

                    m_roomSceneRoots.Add((room, root));

                    root.SetPlayer(m_player);

                    if (m_rooms.Count > 0) loadNextScene();
                    else
                    {
                        yield return new WaitForEndOfFrame();

                        while (m_allDoors.Count > 0)
                        {
                            int ind = -1;
                            Vector3 pos = Vector3.positiveInfinity;
                            for (int i = 0; i < m_allDoors.Count; i++)
                            {
                                Vector3 newPos = m_allDoors[i].transform.position;
                                float dist = Vector3.Distance(pos, newPos);

                                if (pos.x == Mathf.Infinity)
                                    pos = newPos;
                                else
                                {
                                    if (dist < 0.1f)
                                    {
                                        ind = i;
                                        break;
                                    }
                                }
                            }

                            if (ind > 0)
                            {
                                m_allDoors[0].Attach(m_allDoors[ind]);
                                m_allDoors.RemoveAt(ind);
                            }
                            m_allDoors.RemoveAt(0);
                        }

                        m_player.gameObject.SetActive(true);
                        m_finishedGenerating = true;

                        foreach ((Room r, RoomSceneRoot rcr) in m_roomSceneRoots)
                        {
                            rcr.room.AttachDoors();
                        }
                    }
                }
            }
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
            Room[] availableRooms = roomCount < 1 ? m_data.endingRooms : m_data.availableRooms;

            Door doorToAttach = null;
            Room nextRoom = null;
            Door nextDoor = null;
            List<Room> untriedRooms = new List<Room>(availableRooms);

            while (untriedRooms.Count > 0)
            {
                int roomInd = m_random.Next(untriedRooms.Count);
                nextRoom = Instantiate(availableRooms[roomInd], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
                untriedRooms.RemoveAt(roomInd);

                List<Door> untriedNewDoors = new List<Door>(nextRoom.doors);
                while (untriedNewDoors.Count > 0)
                {
                    int ind = m_random.Next(untriedNewDoors.Count);
                    nextDoor = untriedNewDoors[ind];
                    untriedNewDoors.RemoveAt(ind);

                    List<Door> untriedOldDoors = new List<Door>(previousRoom.doors);
                    while (untriedOldDoors.Count > 0)
                    {
                        bool overlap = false;

                        ind = m_random.Next(untriedOldDoors.Count);
                        doorToAttach = untriedOldDoors[ind];
                        untriedOldDoors.RemoveAt(ind);

                        if (Mathf.Abs(doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y) != 180) continue;

                        alignRoomToDoor(nextRoom, nextDoor, doorToAttach);

                        foreach (var room in m_rooms)
                        {
                            bool checkOverlap = nextRoom.volume.CheckVolume(room.volume);
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

                m_rooms.Remove(nextRoom);
                DestroyImmediate(nextRoom.gameObject);
            }
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
