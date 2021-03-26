using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Barrel : MonoBehaviour
    {
        [SerializeField] private GameObject m_muzzleFlash;
        [SerializeField] private float m_speed;

        public void Fire(GunData data)
        {
            Projectile projectile = GameObject.Instantiate(data.projectile, transform.position, transform.rotation);
            projectile.SetData(data);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * m_speed);
            projectile.gameObject.layer = gameObject.layer;

            if (m_muzzleFlash != null)
            {
                GameObject muzzleflash = Instantiate(m_muzzleFlash, transform.position, transform.rotation, transform) as GameObject;
                Destroy(muzzleflash, 1.5f); // Lifetime of muzzle effect.
            }
        }
    }
}
