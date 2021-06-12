using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UntitledFPS
{
    public class FiringAnimator : MonoBehaviour
    {
        public Animator anim;

        public bool isFiring
        {
            get
            {
                return anim.GetCurrentAnimatorStateInfo(0).IsName("Firing");
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Gun.active == false) return;
            if (Input.GetMouseButton(0))
            {
                anim.Play("Firing");
            }
        }
    }
}
