using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class JumpDrop : MonoBehaviour
    {
        [SerializeField] private int m_numJumps;
        public int numJumps { get { return m_numJumps; } }

        private void OnTriggerEnter(Collider other)
        {
            FPSController.PlayerMovement player = other.GetComponentInParent<FPSController.PlayerMovement>();
            if (player != null)
            {
                player.maxExtraJumps += m_numJumps;
                UI.Instance.AddJump();
                Destroy(gameObject);
            }
        }
    }
}
