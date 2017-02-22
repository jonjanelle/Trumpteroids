using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the minelayer style ships
public class SpaceshipController : MonoBehaviour {

    private GameController gameController;

    public GameObject explosion;
    public AudioClip destroy;
    public AudioClip minePlop;
    public GameObject mine;

    private float MoveSpeed = 3.0f;

    private float frequency = 5.0f;  // Speed of sine movement
    private float magnitude = 1.5f;   // Size of sine movement
    private Vector3 axis;
    private Vector3 pos;


    private bool canShoot = true;

    // Use this for initialization
    void Start () {
        // Get a reference to the game controller object and the script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        var startY = Random.Range(-7.5f, 7.5f);
        transform.position = new Vector3(-19, startY, 0);
        pos = transform.position;
        DestroyObject(gameObject, 14.0f);
        axis = transform.up;  
    }
	
	// Update is called once per frame
	void Update () {
        pos += transform.right * Time.deltaTime * MoveSpeed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;

        if (canShoot)
        {
            if (gameController.sfxOn)
            {
                AudioSource.PlayClipAtPoint(minePlop, Camera.main.transform.position);
            }
            var rotation = Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f));
            Destroy(Instantiate(mine, new Vector3(transform.position.x,transform.position.y, 0),rotation),9.0f);
            canShoot = false;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        var reloadTime = Mathf.Max(3.2f - .2f * gameController.wave, .8f);
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Bullet"))
        {
            // Destroy the bullet
            Destroy(c.gameObject);

            //Play explosion animation
            var rotation = Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f));
            Destroy(Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), rotation), .25f);
            if (gameController.sfxOn)
            {
                AudioSource.PlayClipAtPoint(destroy, Camera.main.transform.position);
            }

            // Destroy the current space ship
            Destroy(gameObject);
        }

    }
}
