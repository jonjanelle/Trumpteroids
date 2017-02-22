using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject bigAsteroid1;
    public GameObject bigAsteroid2;
    public GameObject smallAsteroid1;
    public GameObject smallAsteroid2;

    public AudioClip[] backgroundMusic;
    private AudioSource bgMusicSource;
    private int clipNum;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("bgmusic") == 0)
        {
            PlayerPrefs.SetInt("bgmusic", 1);
        }
        if (PlayerPrefs.GetInt("sfx") == 0)
        {
            PlayerPrefs.SetInt("sfx", 1);
        }
        if (PlayerPrefs.GetInt("menuMusic") == 0)
        {
            PlayerPrefs.SetInt("menuMusic", 1);
        }

        clipNum = PlayerPrefs.GetInt("menuMusic");
        clipNum = (clipNum + 1) % 3+1;
        PlayerPrefs.SetInt("menuMusic",clipNum);
        bgMusicSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetInt("bgmusic") != 1)
        {
            bgMusicSource.Stop();
        }
        else
        {
            bgMusicSource.clip=backgroundMusic[clipNum-1];
            bgMusicSource.Play();
        }


        bigAsteroid1 = Instantiate(bigAsteroid1, new Vector3(-7, 2, 0), Quaternion.Euler(0,0,30));
        bigAsteroid1.GetComponent<Rigidbody2D>().AddForce(transform.up * 100);

        bigAsteroid2 = Instantiate(bigAsteroid2, new Vector3(7, -3, 0), Quaternion.Euler(0, 0, -30));
        bigAsteroid2.GetComponent<Rigidbody2D>().AddForce(transform.right * 250);

        smallAsteroid1 = Instantiate(smallAsteroid1, new Vector3(3, -3, 0), Quaternion.Euler(0, 0, 70));
        smallAsteroid1.GetComponent<Rigidbody2D>().AddForce(transform.up * 150);

        smallAsteroid2 = Instantiate(smallAsteroid2, new Vector3(-7, 3, 0), Quaternion.Euler(0, 0, 150));
        smallAsteroid2.GetComponent<Rigidbody2D>().AddForce(transform.right * 180);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
