using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] largeAsteroids;

    public GameObject ship;
    public GameObject mineLayer;
    public GameObject shootShip;

    private int score;
    private int hiscore;
    private int asteroidsRemaining;
    private int lives;
    public int wave;

    private int startCount = 4; //Number of asteroids on first level
    private float increaseEachWave = .5f; //asteroids added per level
    private int waveBannerTime = 3; //Seconds to show the level 

    public Text scoreText;
    public Text livesText;
    public Text waveText;
    public Text hiscoreText;
    public Text waveAnnounce;
    public Text loseText;

    public AudioClip gameStart;
    public AudioClip gameOver;

    public AudioClip[] backgroundMusic;
    public AudioSource bgMusicSource;

    private bool spawnMineShip;
    private bool spawnShootShip;

    public bool bgMusicOn;
    public bool sfxOn; 

    // initialization of a new game
    void Start()
    {        
        hiscore = PlayerPrefs.GetInt("hiscore", 0);
        bgMusicOn = PlayerPrefs.GetInt("bgmusic") == 1;
        sfxOn = PlayerPrefs.GetInt("sfx") == 1;

        bgMusicSource = GetComponent<AudioSource>();
        if (!bgMusicOn) { bgMusicSource.Stop(); }
        else
        {
            AudioSource.PlayClipAtPoint(gameStart, Camera.main.transform.position);
        }
        BeginGame();
    }

    //Sorry world.
    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Quit if player presses escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (wave >= 2 && spawnMineShip)
        {
            Instantiate(mineLayer, new Vector3(0, 0, 0), Quaternion.Euler(0,0,0));
            spawnMineShip = false;
            var lower = Mathf.Max(30 - wave, 5);
            var upper = Mathf.Max(45 - wave, 15);
            StartCoroutine(mineShipSpawnTimer(Random.Range(lower, upper)));
        }
        
        
        if (wave >= 5 && spawnShootShip)
        {
            Instantiate(shootShip, new Vector3(-19, 0, 0), Quaternion.Euler(0, 0, 0));
            spawnShootShip = false;
            var lower = Mathf.Max(30 - wave, 10);
            var upper = Mathf.Max(55 - wave, 20);
            StartCoroutine(shootShipSpawnTimer(Random.Range(lower, upper)));
        }
    }
    //Setup a new game
    void BeginGame()
    {
        //Setup starting player stats
        score = 0;
        lives = 3;
        wave = 1;
        
        // Prepare the HUD
        scoreText.text = "SCORE: " + score;
        hiscoreText.text = "HISCORE: " + hiscore;
        livesText.text = "LIVES: " + lives;
        waveText.text = "WAVE: " + wave;
        loseText.text = "";
        spawnMineShip = false;
        spawnShootShip = false;

        StartCoroutine(ShowWaveBanner(waveBannerTime)); //Announce the next level
        
        SpawnAsteroids(); //create a new set of asteroids
    }

    /**
     * Display a message stating which
     * wave is about to begin.
     */
    IEnumerator ShowWaveBanner(float time)
    {
        if (bgMusicOn)
        {
            changeBGMusic();
        }
        waveAnnounce.text = "BEGIN WAVE " + wave;
        yield return new WaitForSeconds(time);
        waveAnnounce.text = "";
    }

    /**
    * Display game over message
    */
    IEnumerator ShowGameOver(float time)
    {
        loseText.text = "YOU LOSE!";
        yield return new WaitForSeconds(time);
        loseText.text = "";
        ShowAd();
        SceneManager.LoadScene("menu");
    }

    //Timer to control how frequently new spaceships will spawn
    IEnumerator mineShipSpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        spawnMineShip = true;
    }

    //Timer to control how frequently new spaceships will spawn
    IEnumerator shootShipSpawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        spawnShootShip = true;
    }

    //Change the background music
    public void changeBGMusic()
    {
        bgMusicSource.clip = backgroundMusic[(wave-1) % backgroundMusic.Length];
        bgMusicSource.Play();
    }

    //Spawn new random set of asteroids after game start, level change, or game loss
    //Current, spawn new asteroids every other level
    void SpawnAsteroids()
    {
        DestroyAllEnemies();
        // Decide how many asteroids to spawn
        // If any asteroids left over from previous game, subtract them
        asteroidsRemaining = (int)Mathf.Min(startCount + (wave - 1) * increaseEachWave, 15); //At most 15 asteroids at once
        for (int i = 0; i < asteroidsRemaining; i++)
        {
            var rx = 0f;
            var ry = 0f;
            // Spawn an asteroid
            while (Mathf.Abs(rx) < 3.0 && Mathf.Abs(ry) < 3.0f) //So asteroids don't spawn on top of player
            {
                rx = Random.Range(-18.0f, 18.0f);
                ry = Random.Range(-9.0f, 9.0f);
            }
            var rotation = Quaternion.Euler(0, 0, Random.Range(-0.0f, 359.0f));
            //range is 0 to array length
            if (wave < 10) 
            {
                Instantiate(largeAsteroids[Random.Range(0, 4)], new Vector3(rx, ry, 0), rotation);
            }
            else //Include a large asteroid that breaks into 3 large asteroids
            {
                Instantiate(largeAsteroids[Random.Range(0, 5)], new Vector3(rx, ry, 0), rotation);
            }
        }
        //Start timers to determine when enemy ships should spawn
        if (wave >= 2)
            StartCoroutine(mineShipSpawnTimer(Random.Range(5, 15)));
        if (wave >= 5)
            StartCoroutine(shootShipSpawnTimer(Random.Range(5, 15)));

        waveText.text = "WAVE: " + wave;
    }

    //Increase player score and check whether
    //a new high score has been reached
    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE:" + score;
        if (score%100 == 0) //Gain a life for each 100 points
        {
            lives++;
            livesText.text = "LIVES: " + lives;
        }

        if (score > hiscore)
        {
            hiscore = score;
            hiscoreText.text = "HISCORE: " + hiscore;

            // Save the new high score
            PlayerPrefs.SetInt("hiscore", hiscore);
        }
    }

    //Advance to the next wave. 
    //Adds a delay between waves and launches a message showing the next wave
    IEnumerator waveFinished(float time)
    {
        wave++;

        ship.transform.position = new Vector3(0f, 0f, 0f); //Move player ship back to center
        ship.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0); //remove all velocity from player ship

        StartCoroutine(ShowWaveBanner(waveBannerTime));
        yield return new WaitForSeconds(time); //So we don't spawn new asteroids until banner is done displaying

        SpawnAsteroids();
    }


    //Decrement player life after collision with enemy
    //object and check whether player should lose
    public void DecrementLives()
    {
        lives--;
        livesText.text = "LIVES: " + lives;

        // Has player run out of lives?
        if (lives < 1)
        {
            if (sfxOn)
            {
                AudioSource.PlayClipAtPoint(gameOver, Camera.main.transform.position);
            }
            DestroyAllEnemies();
            
            // Restart the game
            StartCoroutine(ShowGameOver(5));
        }
    }

    //Reduce the number of remaining asteroids by 1
    public void DecrementAsteroids()
    {
        asteroidsRemaining--;
        if (asteroidsRemaining < 1)
        {
            StopAllCoroutines();
            DestroyAllEnemies();
            StartCoroutine(waveFinished(waveBannerTime));
        }
    }

    //Change asteroid count after a large asteroid has been broken
    public void SplitAsteroid()
    {
        // Two extra asteroids since large destroyed and 
        // three small added
        asteroidsRemaining += 2;
    }

    //Destroy all existing asteroids on the screen
    void DestroyExistingAsteroids()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Large Asteroid");

        foreach (GameObject current in asteroids)
        {
            GameObject.Destroy(current);
        }

        GameObject[] asteroids2 = GameObject.FindGameObjectsWithTag("Small Asteroid");

        foreach (GameObject current in asteroids2)
        {
            GameObject.Destroy(current);
        }
    }

    void DestroyAllEnemies()
    {
        DestroyExistingAsteroids();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy Ship");
        foreach (GameObject x in enemies)
        {
            GameObject.Destroy(x);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Enemy Bullet");
        foreach (GameObject x in bullets)
        {
            GameObject.Destroy(x);
        }
    }

}