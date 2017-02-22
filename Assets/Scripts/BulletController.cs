using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // Set the bullet to destroy itself after 1 seconds
        Destroy(gameObject, 2.25f);

        // Push the bullet in the direction it is facing
        GetComponent<Rigidbody2D>().AddForce(transform.up * 425);
    }

}