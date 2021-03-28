using UnityEngine;
using System.Collections;

public class NoCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
