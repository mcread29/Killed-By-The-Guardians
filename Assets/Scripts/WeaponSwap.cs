using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class WeaponSwap : MonoBehaviour
    {
        public GameObject m_weapon1;
        public GameObject m_weapon2;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown("1"))
            {
                print("Weapon 1 is Active");
                m_weapon1.SetActive(true);
                m_weapon2.SetActive(false);

            }
            else if (Input.GetKeyDown("2"))
            {
                print("Weapon 2 is Active");
                m_weapon1.SetActive(false);
                m_weapon2.SetActive(true);
            }
        }
    }
}
