using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UntitledFPS
{
    public class RoomSceneRoot : MonoBehaviour
    {
        // [SerializeField] private GameObject m_lighting;
        [SerializeField] private Room m_room;
        public Room room { get { return m_room; } }

        [SerializeField] private RoomSection[] m_sections;

        private int m_sectionsComplete = 0;

        [SerializeField] private GameObject m_staticObjects;

        [SerializeField] private Player m_player;
        public Player player
        {
            get { return m_player; }
            set { m_player = value; }
        }

        private void Awake()
        {
            m_sectionsComplete = 0;
        }

        private void Start()
        {
            if (Generator.Instance == null)
                SetPlayer(m_player);
        }

        public void SetPlayer(Player player)
        {
            if (m_player != null && player != m_player) DestroyImmediate(m_player.gameObject);

            m_player = player;
            if (m_sections != null)
            {
                foreach (RoomSection section in m_sections)
                {
                    if (player != null) section.SetTurretLookAt(player.transform);
                    section.sectionComplete += sectionComplete;
                    section.sectionStarted += roomStarted;
                    section.sectionStarted += player.SectionStarted;
                }
            }
        }

        private void sectionComplete(RoomSection section)
        {
            section.sectionComplete -= sectionComplete;
            section.sectionStarted -= player.SectionStarted;
            m_sectionsComplete++;
            if (m_sectionsComplete >= m_sections.Length)
            {
                foreach (RoomSection roomSection in m_sections)
                    roomSection.StopShooting();
                m_room.FinishRoom();
                player.RoomFinished();
            }
        }

        private void roomStarted()
        {
            foreach (RoomSection section in m_sections)
            {
                section.sectionStarted -= roomStarted;
                StartCoroutine(section.startSectionTimer());
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Bake Lighting")]
        public void BakeLighting()
        {
            m_staticObjects.isStatic = true;
            foreach (Transform o in m_staticObjects.transform)
                o.gameObject.isStatic = true;

            Lightmapping.bakeCompleted += setNonStatic;
            // Lightmapping.

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
#endif
    }
}
