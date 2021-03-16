using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    [ExecuteInEditMode]
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform m_barrelParent;
        [SerializeField] private Transform m_playerTransform;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        [ExecuteInEditMode]
        void Update()
        {
            if (m_barrelParent != null && m_playerTransform != null)
            {
                Quaternion prevRotation = m_barrelParent.rotation;
                m_barrelParent.LookAt(m_playerTransform);
                Vector3 rotation = m_barrelParent.transform.localRotation.eulerAngles;
                if (rotation.x < 270 || rotation.y > 360) m_barrelParent.rotation = prevRotation;
                // rotation.x = Mathf.Clamp(rotation.x, 270, 360);
                // m_barrelParent.transform.rotation = Quaternion.Euler(rotation);
            }
        }
    }
}
