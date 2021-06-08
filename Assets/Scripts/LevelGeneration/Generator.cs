using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;

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

        private Dictionary<string, int> m_roomCounts;

        private List<RoomSceneRoot> m_startRooms;
        private List<RoomSceneRoot> m_availableRooms;
        private List<RoomSceneRoot> m_bossRooms;

        float startTime;
        private string[] possibleRotations = { "0", "90", "180", "270" };
        private IEnumerator getAssetBundle()
        {
            startTime = Time.time;
            Debug.Log($"STARTING COROUTINE {startTime}");
            AssetBundleCreateRequest a = AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, "level1"));
            Debug.Log(a);
            yield return a;

            AssetBundle b = a.assetBundle;

            if (b == null)
            {
                Debug.LogError("YA FUCKED UP");
                yield break;
            }
            Debug.Log($"{b} {Time.time - startTime}");

            m_startRooms = new List<RoomSceneRoot>();
            foreach (string folder in m_data.startRoomFolders)
            {
                foreach (string r in possibleRotations)
                {
                    AssetBundleRequest req = b.LoadAssetAsync<GameObject>($"{folder}.{r}");
                    yield return req;
                    GameObject o = req.asset as GameObject;
                    if (o != null) m_startRooms.Add(o.GetComponent<RoomSceneRoot>());
                }
            }

            m_availableRooms = new List<RoomSceneRoot>();
            foreach (string folder in m_data.availableRoomFolders)
            {
                foreach (string r in possibleRotations)
                {
                    AssetBundleRequest req = b.LoadAssetAsync<GameObject>($"{folder}.{r}");
                    yield return req;
                    GameObject o = req.asset as GameObject;
                    if (o != null) m_availableRooms.Add(o.GetComponent<RoomSceneRoot>());
                }
            }

            m_bossRooms = new List<RoomSceneRoot>();
            foreach (string folder in m_data.bossRoomFolders)
            {
                foreach (string r in possibleRotations)
                {
                    AssetBundleRequest req = b.LoadAssetAsync<GameObject>($"{folder}.{r}");
                    yield return req;
                    GameObject o = req.asset as GameObject;
                    if (o != null) m_bossRooms.Add(o.GetComponent<RoomSceneRoot>());
                }
            }

            b.Unload(false);

            Debug.Log($"ASSETS LOADED {Time.time - startTime} {m_startRooms.Count} {m_availableRooms.Count} {m_bossRooms.Count}");
            Generate();
        }

        private void Awake()
        {
            if (m_instance != null)
                Destroy(gameObject);
            else
            {
                m_instance = this;

                Turret.HealthDropRate = m_data.turretHealthDropRate;
                Turret.HealthDrop = m_data.healthDropPrefab;
                Turret.JumpDropRate = m_data.turretJumpDropRate;
                Turret.JumpDrop = m_data.jumpDropPrefab;
            }
        }

        private void Start()
        {
            StartCoroutine(getAssetBundle());
        }

        [ContextMenu("GENERATE")]
        public void Generate()
        {
            Debug.Log($"Starting Generation at {Time.time - startTime}");
            m_roomCounts = new Dictionary<string, int>();

            int numRooms = m_numRooms = Random.Range(m_data.minLength, m_data.maxLength);

            foreach (RoomSceneRoot room in m_startRooms)
            {
                if (m_roomCounts.ContainsKey(room.room.name) == false)
                    m_roomCounts.Add(room.room.name, 0);
            }
            foreach (RoomSceneRoot room in m_availableRooms)
            {
                if (m_roomCounts.ContainsKey(room.room.name) == false)
                    m_roomCounts.Add(room.room.name, 0);
            }
            foreach (RoomSceneRoot room in m_bossRooms)
            {
                if (m_roomCounts.ContainsKey(room.room.name) == false)
                    m_roomCounts.Add(room.room.name, 0);
            }

            bool successfullGeneration = false;
            int attempts = 0;
            while (successfullGeneration == false && attempts < m_numAttempts)
            {
                Debug.Log("ATTEMPT " + attempts);
                if (m_useCustomSeed == false || m_randomSeed == -1) m_randomSeed = new System.Random().Next();
                Random.InitState(m_randomSeed);

                m_rooms = new List<RoomSceneRoot>();
                RoomSceneRoot startRoom = addStartRoom();
                successfullGeneration = newRoom(startRoom, numRooms);

                if (successfullGeneration == false)
                {
                    m_rooms.Remove(startRoom);
                    m_roomCounts[startRoom.room.name]--;
                    DestroyImmediate(startRoom.gameObject);
                    attempts++;
                }
            }

            foreach (RoomSceneRoot root in m_rooms)
            {
                if (root.player != null && m_player == null)
                {
                    m_player = root.player;
                    m_player.gameObject.SetActive(false);
                }
                else
                    root.SetPlayer(m_player);

                root.room.AttachDoors();
            }
            //if faild it will fail
            Debug.Log($"Finished Generation at {Time.time - startTime} {successfullGeneration}");
            m_player.gameObject.SetActive(true);
            m_finishedGenerating = true;
        }

        private RoomSceneRoot addStartRoom()
        {
            int ind = Random.Range(0, m_startRooms.Count);
            RoomSceneRoot startRoom = Instantiate(m_startRooms[ind], Vector3.zero, m_startRooms[ind].transform.rotation, transform);
            m_rooms.Add(startRoom);
            m_roomCounts[startRoom.room.name]++;
            return startRoom;
        }

        private string getMostUsedRoom()
        {
            KeyValuePair<string, int> mostUsed = new KeyValuePair<string, int>("", -1);
            foreach (KeyValuePair<string, int> pair in m_roomCounts)
            {
                if (pair.Value > mostUsed.Value) mostUsed = pair;
            }
            return mostUsed.Key;
        }

        private int getLeastUsedRoom()
        {
            KeyValuePair<string, int> leastUsed = new KeyValuePair<string, int>("", 1000);
            foreach (KeyValuePair<string, int> pair in m_roomCounts)
            {
                if (pair.Value < leastUsed.Value) leastUsed = pair;
            }
            return leastUsed.Value;
        }

        private bool newRoom(RoomSceneRoot previousRoom, int roomCount)
        {
            bool finalRoom = roomCount < 1;
            List<RoomSceneRoot> availableRooms = finalRoom ? m_bossRooms : m_availableRooms;

            List<RoomSceneRoot> untriedRooms = new List<RoomSceneRoot>(availableRooms);

            while (untriedRooms.Count > 0)
            {
                int roomInd = Random.Range(0, untriedRooms.Count);
                RoomSceneRoot nextRoom = Instantiate(untriedRooms[roomInd], Vector3.zero, untriedRooms[roomInd].transform.rotation, transform);
                untriedRooms.RemoveAt(roomInd);

                bool sameAsPreviousRoom = nextRoom.room.name == previousRoom.room.name;
                bool isMostUsedRoom = getMostUsedRoom() == nextRoom.room.name;
                bool usedWayMoreThanLeastRoom = getLeastUsedRoom() + 2 < m_roomCounts[nextRoom.room.name];

                if (finalRoom == false)
                {
                    m_roomCounts[nextRoom.room.name]++;
                }

                m_rooms.Add(nextRoom);
                if (sameAsPreviousRoom == false && isMostUsedRoom == false && usedWayMoreThanLeastRoom == false)
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

                            if (Mathf.Abs(MyMath.Round(doorToAttach.transform.rotation.eulerAngles.y, 1) - MyMath.Round(nextDoor.transform.rotation.eulerAngles.y, 1)) != 180) continue;

                            alignRoomToDoor2(nextRoom, nextDoor, doorToAttach);

                            foreach (var roomRoot in m_rooms)
                            {
                                if (roomRoot == nextRoom) continue;
                                bool checkOverlap = nextRoom.room.volume.CheckVolume(roomRoot.room.volume);
                                overlap = overlap || checkOverlap;
                                if (overlap) break;
                            }

                            if (overlap) continue;
                            else
                            {
                                bool success = finalRoom || newRoom(nextRoom, roomCount - 1);
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
                if (finalRoom == false)
                {
                    m_roomCounts[nextRoom.room.name]--;
                }
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
