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
        [SerializeField] private int m_numRooms = 10;
        [SerializeField] private LevelData m_data;

        private System.Random m_random;
        private List<Room> m_rooms;

        private Player m_player;

        [ContextMenu("Generate")]
        public void Generate()
        {
            m_rooms = new List<Room>();
            m_random = new System.Random();

            int numRooms = Random.Range(m_data.minLength, m_data.maxLength);
            Room startRoom = addStartRoom();

            bool successfullGeneration = false;
            while (successfullGeneration == false)
            {
                successfullGeneration = newRoom(startRoom, numRooms);
            }

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
            Destroy(m_rooms[0].gameObject);
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
                    if (m_rooms.Count > 0) loadNextScene();
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

                    if (root.player != null && m_player == null) m_player = root.player;
                    else if (root.player != null)
                    {
                        Destroy(root.player.gameObject);
                        root.SetPlayer(m_player);
                    }

                    for (int i = 0; i < room.doors.Length; i++)
                    {
                        Door door = room.doors[i];
                        if (door.attached)
                            root.room.doors[i].Attach();
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
                            overlap = overlap || nextRoom.volume.CheckVolume(room.volume);
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
