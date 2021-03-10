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
        Room2
    }

    public class Generator : MonoBehaviour
    {
        [SerializeField] private LevelData m_data;

        private System.Random m_random;
        private List<Room> m_rooms;
        private List<RoomSceneData> m_scenes;

        [ContextMenu("Generate")]
        public void Generate()
        {
            m_rooms = new List<Room>();
            m_scenes = new List<RoomSceneData>();
            m_random = new System.Random();

            int mumRooms = Random.Range(m_data.minLength, m_data.maxLength);
            int ind = Random.Range(0, m_data.startRooms.Length);
            Room startRoom = Instantiate(m_data.startRooms[ind].room, Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            m_scenes.Add(new RoomSceneData(startRoom, m_data.startRooms[ind].roomName, startRoom.transform.position));
            m_rooms.Add(startRoom);
            newRoom(startRoom, 4);

            loadNextScene();
        }

        private EditorSceneManager.SceneOpenedCallback roomSceneOpened(Vector3 position, string sceneName)
        {
            EditorSceneManager.SceneOpenedCallback a = null;
            a = (Scene scene, OpenSceneMode l) =>
            {
                if (scene.name == sceneName)
                {
                    EditorSceneManager.sceneOpened -= a;
                    StartCoroutine(moveAfterLoad(scene, position));
                }
            };
            return a;
        }

        private void loadNextScene()
        {
            Destroy(m_rooms[0].gameObject);
            m_rooms.RemoveAt(0);

            RoomSceneData data = m_scenes[0];
            m_scenes.RemoveAt(0);

            string roomName = data.roomName.ToString();
            SceneManager.LoadScene(roomName, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += roomSceneLoaded(data.position, roomName);
        }

        private UnityAction<Scene, LoadSceneMode> roomSceneLoaded(Vector3 position, string sceneName)
        {
            UnityAction<Scene, LoadSceneMode> a = null;
            a = (Scene scene, LoadSceneMode l) =>
            {
                if (scene.name == sceneName)
                {
                    StartCoroutine(moveAfterLoad(scene, position));
                    SceneManager.sceneLoaded -= a;
                    if (m_scenes.Count > 0) loadNextScene();
                }
            };
            return a;
        }

        private IEnumerator moveAfterLoad(Scene scene, Vector3 position)
        {
            while (scene.isLoaded == false) yield return new WaitForEndOfFrame();

            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
                rootGameObject.transform.position = position;
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
            RoomSceneData data = null;
            List<RoomSceneData> untriedRooms = new List<RoomSceneData>(m_data.availableRooms);

            do
            {
                int roomInd = m_random.Next(untriedRooms.Count);
                nextRoom = Instantiate(m_data.availableRooms[roomInd].room, Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
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
                            data = new RoomSceneData(nextRoom, m_data.availableRooms[roomInd].roomName, nextRoom.transform.position);
                            m_scenes.Add(data);
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

                m_scenes.Remove(data);
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
