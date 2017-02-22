using UnityEngine;
using System.Collections;


public class AsteroidController : MonoBehaviour
{
    public AudioClip destroy;
    public AudioClip smallDestroy;
    //Begin declaration of random clips played on large asteroid break
    public AudioClip[] clips;

    public GameObject explosion;
    public GameObject bloodExplosion;

    public GameObject smallAsteroid;

    private GameController gameController;

    // Initialize new asteroids
    void Start()
    {
        // Get a reference to the game controller object and the script
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
       if (gameController != null)
       {
            // Push the asteroid in the direction it is facing
            var forceMult = 0f;
            if (tag.Equals("Large Asteroid"))
            {
                forceMult = Random.Range(-160.0f - 8 * gameController.wave, 160.0f + 8 * gameController.wave);
            }
            else //make small asteroids go faster
            {
                forceMult = Random.Range(-190.0f - 8 * gameController.wave, 190.0f + 8 * gameController.wave);
            }
            GetComponent<Rigidbody2D>().AddForce(transform.up * forceMult);

            // Give a random angular velocity/rotation
            GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-0.0f, 90.0f);
        }



    }


   
    //Describe behavior of bullet hitting asteroid
    //The asteroid struck is destroyed. If a large asteroid
    //is struck, then three smaller ones are spawned
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag.Equals("Bullet"))
        {
            // Destroy the bullet
            Destroy(c.gameObject);


            // If large asteroid spawn new ones
            if (tag.Equals("Large Asteroid"))
            {
                //Play explosion animation
                var rotation = Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f));
                Destroy(Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), rotation), .4f);

                // Spawn small asteroid #1
                var newX = transform.position.x - .75f;
                var newY = transform.position.y - .75f;
                Instantiate(smallAsteroid, new Vector3(newX, newY, 0), Quaternion.Euler(0, 0, 90));
                newX += 1f;
                newY +=.75f;
                // Spawn small asteroid #2
                Instantiate(smallAsteroid, new Vector3(newX,newY, 0), Quaternion.Euler(0, 0, 0));

                // Spawn small asteroid #3
                newY -= 1.0f;
                Instantiate(smallAsteroid, new Vector3(newX, newY, 0), Quaternion.Euler(0, 0, 270));

                gameController.SplitAsteroid(); // +2
                if (gameController.sfxOn)
                {
                    var playSound = Random.Range(0.0f, 1.0f);
                    if (playSound > .65)
                    {
                        AudioSource.PlayClipAtPoint(clips[Random.Range(0, 14)], Camera.main.transform.position);
                    }
                    else
                    {
                        AudioSource.PlayClipAtPoint(destroy, Camera.main.transform.position);
                    }
                }

            }
            else
            {
                //Play blood spatter animation when small asteroid destroyed
                //Play explosion animation
                var rotation = Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f));
                Destroy(Instantiate(bloodExplosion, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), rotation), .20f);

                // Just a small asteroid destroyed
                gameController.DecrementAsteroids();
                if (gameController.sfxOn)
                {
                    AudioSource.PlayClipAtPoint(smallDestroy, Camera.main.transform.position);
                }
            }
            
            // Add to the score
            gameController.IncrementScore();

            // Destroy the current asteroid
            Destroy(gameObject);
        }

    }
}