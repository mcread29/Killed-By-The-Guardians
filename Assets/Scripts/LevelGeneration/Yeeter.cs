using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Yeeter : MonoBehaviour
    {
        [SerializeField] private float m_yeetForce = 1500;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("WOWZA");
            if (other.tag == "Player")
            {
                Debug.Log("YEET");
                other.transform.parent.GetComponent<FPSController.PlayerMovement>().ForceJump(m_yeetForce);
            }
        }
    }
}
