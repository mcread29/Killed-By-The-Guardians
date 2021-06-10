using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringAnimator : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
    }

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
        if (Input.GetMouseButton(0))
        {
            anim.Play("Firing");
        }
    }
}
