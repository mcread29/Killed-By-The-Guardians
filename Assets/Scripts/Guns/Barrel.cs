using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Barrel : MonoBehaviour
    {
        public void Fire(GunData data)
        {
            Projectile projectile = GameObject.Instantiate(data.projectile, transform.position, transform.rotation);
            projectile.SetData(data);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * data.shotSpeed);
            projectile.gameObject.layer = gameObject.layer;

            if (data.muzzleFlash != null)
            {
                GameObject muzzleflash = Instantiate(data.muzzleFlash, transform.position, transform.rotation, transform) as GameObject;
                Destroy(muzzleflash, 1.5f); // Lifetime of muzzle effect.
            }
        }
    }
}
