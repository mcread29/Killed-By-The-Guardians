using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class RoomSceneRoot : MonoBehaviour
    {
        [SerializeField] private GameObject m_lighting;
        [SerializeField] private Room m_room;
        public Room room { get { return m_room; } }

        [SerializeField] private RoomSection[] m_sections;

        [SerializeField] private GameObject m_staticObjects;

        [SerializeField] private Player m_player;
        public Player player
        {
            get { return m_player; }
            set { m_player = value; }
        }

        private void Awake()
        {
            if (m_sections != null)
            {
                foreach (RoomSection section in m_sections)
                {
                    section.gameObject.SetActive(false);
                }
            }
            bool inGenerator = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == "LevelGeneration") inGenerator = true;
            }

            if (inGenerator)
            {
                DestroyLighting();
            }
            else
            {
                StartRoom();
            }
        }

        public void SetPlayer(Player player)
        {
            m_player = player;
            if (m_sections != null)
            {
                foreach (RoomSection section in m_sections)
                {
                    section.SetTurretLookAt(player.transform);
                }
            }
        }

        public void StartRoom()
        {
            if (m_sections != null)
            {
                foreach (RoomSection section in m_sections)
                {
                    section.SetStarted();
                }
            }
        }

        public void DestroyLighting()
        {
            Destroy(m_lighting.gameObject);
        }

        [ContextMenu("Bake Lighting")]
        public void BakeLighting()
        {
            m_staticObjects.isStatic = true;
            foreach (Transform o in m_staticObjects.transform)
                o.gameObject.isStatic = true;

            Lightmapping.bakeCompleted += setNonStatic;

            Lightmapping.Clear();
            bool worked = Lightmapping.BakeAsync();
        }

        private void setNonStatic()
        {
            m_staticObjects.isStatic = false;
            foreach (Transform o in m_staticObjects.transform)
                o.gameObject.isStatic = false;

            Lightmapping.bakeCompleted -= setNonStatic;
        }
    }
}
