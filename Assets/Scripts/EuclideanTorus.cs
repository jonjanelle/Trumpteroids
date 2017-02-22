using UnityEngine;
using System.Collections;

public class EuclideanTorus : MonoBehaviour
{

    private float xBound = 15.5f;
    private float yBound = 9.0f;
    // Update is called once per frame
    void Update()
    {

        // Teleport the game object
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, 0);
        }

        else if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, 0);
        }

        else if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, -yBound, 0);
        }

        else if (transform.position.y < -yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, 0);
        }
    }
}