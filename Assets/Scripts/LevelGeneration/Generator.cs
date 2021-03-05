using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] private LevelData m_data;

        private System.Random m_random;

        [ContextMenu("Generate")]
        public void Generate()
        {
            m_random = new System.Random();
            Debug.ClearDeveloperConsole();
            int mumRooms = Random.Range(m_data.minLength, m_data.maxLength);
            Room startRoom = Instantiate(m_data.startRooms[Random.Range(0, m_data.startRooms.Length)], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            addRoom(startRoom, 15);
        }

        private void addRoom(Room previousRoom, int numberOfRooms)
        {
            Door doorToAttach = previousRoom.pickDoor(m_data.straightness, m_random);

            Room nextRoom = Instantiate(m_data.availableRooms[Random.Range(0, m_data.availableRooms.Length)], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            Door nextDoor = nextRoom.ChooseDoor();

            Vector3 nextLocalPos = nextDoor.transform.localPosition;

            nextRoom.transform.Rotate(new Vector3(0, doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y + 180, 0));

            nextLocalPos = Quaternion.Euler(nextRoom.transform.rotation.eulerAngles) * (nextLocalPos - Vector3.zero) + Vector3.zero;

            nextRoom.transform.position = doorToAttach.transform.position - nextLocalPos;

            //FINALIZE ROOM
            doorToAttach.Attach(nextDoor);
            nextDoor.gameObject.SetActive(false);
            doorToAttach.gameObject.SetActive(false);

            numberOfRooms--;
            if (numberOfRooms > 0) addRoom(nextRoom, numberOfRooms);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            if (transform.childCount > 0) Clear();
        }
    }
}
