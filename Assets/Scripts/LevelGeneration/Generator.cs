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
            addRoom(startRoom.ChooseDoor(), 15);
        }

        private void addRoom(Door doorToAttach, int numberOfRooms)
        {
            Room nextRoom = Instantiate(m_data.availableRooms[Random.Range(0, m_data.availableRooms.Length)], Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            Door nextDoor = nextRoom.ChooseDoor();
            doorToAttach.Attach(nextDoor);

            Vector3 nextLocalPos = nextDoor.transform.localPosition;

            nextRoom.transform.Rotate(new Vector3(0, doorToAttach.transform.rotation.eulerAngles.y - nextDoor.transform.rotation.eulerAngles.y + 180, 0));

            nextLocalPos = RotatePointAroundPivot(nextLocalPos, Vector3.zero, nextRoom.transform.rotation.eulerAngles);

            nextRoom.transform.position = doorToAttach.transform.position - nextLocalPos;
            nextDoor.gameObject.SetActive(false);
            doorToAttach.gameObject.SetActive(false);

            numberOfRooms--;
            if (numberOfRooms > 0) addRoom(pickDoor(nextDoor, m_data.straightness, nextRoom.doors), numberOfRooms);
        }

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }

        private Door pickDoor(Door previusAttached, float straightness, Door[] doors)
        {
            int angleToMatch = Random.Range(0f, 1f) > straightness ? 90 : 180;

            Door selectedDoor = null;
            List<Door> unTried = new List<Door>(doors);
            while (unTried.Count > 0 && selectedDoor == null)
            {
                int ind = m_random.Next(unTried.Count);
                Door d = unTried[ind];
                unTried.RemoveAt(ind);
                if (d.attached == false && (int)Mathf.Abs(previusAttached.transform.eulerAngles.y - d.transform.eulerAngles.y) % angleToMatch == 0)
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

        [ContextMenu("Clear")]
        public void Clear()
        {
            GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            if (transform.childCount > 0) Clear();
        }
    }
}
