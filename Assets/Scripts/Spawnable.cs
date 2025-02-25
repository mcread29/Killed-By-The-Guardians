using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class Spawnable : MonoBehaviour
    {
        public void Spawn()
        {
            gameObject.SetActive(true);
            Health health = gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.FullHeal();
            }
        }

        public void Despawn()
        {
            Disolver disolver = GetComponent<Disolver>();
            if (disolver != null)
            {
                disolver.DisolveOut((t) => gameObject.SetActive(false));
            }
        }
    }
}
