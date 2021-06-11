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
            Player player = other.GetComponentInParent<Player>();
            if (player != null)
            {
                player.AddJump();
                Destroy(gameObject);
            }
        }
    }
}
