using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleButtonController : MonoBehaviour {
    public Toggle bgMusicToggle;
    public Toggle sfxMusicToggle;
    public AudioClip[] backgroundMusic;

    private AudioSource bgMusicSource;
    private bool musicOn;
    private bool soundOn;

	// Use this for initialization
	void Start () {
        musicOn = bgMusicToggle.isOn;
        soundOn = sfxMusicToggle.isOn;
        bgMusicSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetInt("bgmusic") != 1) 
        {
            bgMusicToggle.isOn = false;
            bgMusicSource.Stop();
            
        }
        else 
        {
            bgMusicSource.clip = backgroundMusic[PlayerPrefs.GetInt("menuMusic")-1];
            bgMusicSource.Play();
        }
        if (PlayerPrefs.GetInt("sfx") != 1)
        {
            sfxMusicToggle.isOn = false;
        }

    }

	// Update is called once per frame
	void Update () {

        if (bgMusicToggle.isOn != musicOn)
        {
            if (bgMusicToggle.isOn)
            {
                PlayerPrefs.SetInt("bgmusic", 1);
                bgMusicSource.Play();
                
                //turn music on
            }
            else
            {
                PlayerPrefs.SetInt("bgmusic", -1);
                bgMusicSource.Stop();
            }
            musicOn = bgMusicToggle.isOn;
        }

        if (sfxMusicToggle.isOn != soundOn)
        {
            if (sfxMusicToggle.isOn)
            {
                PlayerPrefs.SetInt("sfx", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sfx", -1);
            }
            soundOn = sfxMusicToggle.isOn;
        }
    }
}
