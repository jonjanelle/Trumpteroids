using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController2 : MonoBehaviour {

    public GameObject explosion;
    public AudioClip destroy;
    public AudioClip shootSound;
    public GameObject bullet;

    private GameController gameController;
    private float speed = 2.5f;
    private Vector3 pos;
    private bool canShoot = true;
    private int direction; 
    // Use this for initialization
    void Start () {
        // Get a reference to the game controller object and the script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    
        var startY = Random.Range(-8.0f, 8.0f);
        if (gameController.wave >= 15)
        {
            if (Random.Range(0f, 1f) > .5f)
            {
                direction = 1; 
                transform.position = new Vector3(-19, startY, 0);
            }
            else
            {
                direction = -1;
                transform.position = new Vector3(19, startY, 0);
            }
        }
        else
        {
            direction = 1;
            transform.position = new Vector3(-19, startY, 0);
        }

        //Set speed and life length to increase with higher waves.
        speed = speed + 0.03f * gameController.wave;
        DestroyObject(gameObject, Mathf.Max(gameController.wave*.5f, 20.0f));
    }
	
	// Update is called once per frame
	void Update () {
        float newX;
        if (direction == 1)
        {
            newX = transform.position.x + speed * Time.deltaTime;
        }
        else
        {
            newX = transform.position.x - speed * Time.deltaTime;
        }
        transform.position = new Vector3(newX, transform.position.y,0);

        if (canShoot)
        {
            if (gameController.sfxOn)
            {
                AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position);
            }
            Instantiate(bullet, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
            canShoot = false;
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(Mathf.Max(15-gameController.wave*.5f,1));
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
