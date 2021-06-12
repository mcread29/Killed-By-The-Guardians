using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private bool m_godMode = false;
        private FPSController.PlayerMovement m_movement;
        private Health m_health;
        private PlayerSounds m_sounds;

        private float m_startTime = 0;

        private void Awake()
        {
            m_movement = GetComponent<FPSController.PlayerMovement>();
            m_health = GetComponent<Health>();
            m_sounds = GetComponent<PlayerSounds>();
            MoveToLayer(transform, gameObject.layer);
        }

        private void Start()
        {
            m_health.onDeath += Death;

            m_movement.jump += m_sounds.Jump;
            m_movement.jump += UI.Instance.Jump;
            m_movement.jumpReset += UI.Instance.JumpReset;

            BossRoom.bossFinished += Win;

            m_startTime = Time.time;
        }

        private void Death()
        {
            if (m_godMode) return;

            removeListeners();

            m_movement.Lock();
            UI.Instance.PlayerKilled();
        }

        public void Win()
        {
            removeListeners();

            m_movement.Lock();
            UI.Instance.PlayerWon(Time.time - m_startTime);
        }

        private void removeListeners()
        {
            m_health.onDeath -= Death;

            m_movement.jump -= m_sounds.Jump;
            m_movement.jump -= UI.Instance.Jump;
            m_movement.jumpReset -= UI.Instance.JumpReset;

            BossRoom.bossFinished -= Win;
        }

        public void AddJump()
        {
            m_movement.maxExtraJumps += 1;
            m_sounds.JumpPickup();
            UI.Instance.AddJump();
        }

        public void AddHealth(int healAmount)
        {
            m_sounds.HealthPickup();
            m_health.Heal(healAmount);
        }

        public void SectionStarted()
        {
            m_sounds.Spawn();
        }

        public void RoomFinished()
        {
            m_sounds.NextRoom();
        }

        public void Kill()
        {
            m_sounds.Kill();
        }

        void MoveToLayer(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                MoveToLayer(child, layer);
        }
    }
}
