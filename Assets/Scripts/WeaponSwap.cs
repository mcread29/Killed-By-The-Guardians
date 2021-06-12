using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class WeaponSwap : MonoBehaviour
    {
        [SerializeField] private Gun[] m_weapons;
        private int m_activeWeapon = 0;

        private GoTweenConfig m_activateConfig;
        private GoTweenConfig m_deactivateConfig;

        private bool m_switching = false;

        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < m_weapons.Length; i++)
            {
                if (i == m_activeWeapon)
                {
                    m_weapons[i].transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                    m_weapons[i].Enable();
                }
                else
                {
                    m_weapons[i].transform.parent.localRotation = Quaternion.Euler(180, 0, 0);
                    m_weapons[i].Disable();
                }
            }
            m_activateConfig = new GoTweenConfig();
            m_activateConfig.localRotation(new Vector3(0, 0, 0));
            m_activateConfig.setEaseType(GoEaseType.QuintOut);

            m_deactivateConfig = new GoTweenConfig();
            m_deactivateConfig.localRotation(new Vector3(180, 0, 0));
            m_activateConfig.setEaseType(GoEaseType.QuintOut);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_switching || Gun.active == false) return;

            if (m_activeWeapon != 1 && Input.GetKeyDown("1") && m_weapons[0].GetComponent<FiringAnimator>().isFiring == false)
            {
                print("Weapon 1 is Active");
                m_switching = true;
                m_activeWeapon = 1;
                m_weapons[1].Disable();
                m_activateConfig.onCompleteHandler = null;
                m_activateConfig.onComplete((t) =>
                {
                    m_weapons[0].Enable();
                    m_switching = false;
                });
                Go.to(m_weapons[0].transform.parent, .15f, m_activateConfig);
                Go.to(m_weapons[1].transform.parent, .15f, m_deactivateConfig);
            }
            else if (m_activeWeapon != 2 && Input.GetKeyDown("2") && m_weapons[1].GetComponent<FiringAnimator>().isFiring == false)
            {
                print("Weapon 2 is Active");
                m_switching = true;
                m_activeWeapon = 2;
                m_weapons[0].Disable();
                m_activateConfig.onCompleteHandler = null;
                m_activateConfig.onComplete((t) =>
                {
                    m_weapons[1].Enable();
                    m_switching = false;
                });
                Go.to(m_weapons[1].transform.parent, .15f, m_activateConfig);
                Go.to(m_weapons[0].transform.parent, .15f, m_deactivateConfig);
            }
        }
    }
}
