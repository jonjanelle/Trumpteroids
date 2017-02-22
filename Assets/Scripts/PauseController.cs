using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour {

    public Toggle bgMusicToggle;
    public Toggle sfxMusicToggle;

    private bool musicOn;
    private bool soundOn;

    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        musicOn = bgMusicToggle.isOn;
        soundOn = sfxMusicToggle.isOn;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        if (PlayerPrefs.GetInt("bgmusic") != 1)
        {
            bgMusicToggle.isOn = false;
        }

        if (PlayerPrefs.GetInt("sfx") != 1)
        {
            sfxMusicToggle.isOn = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (bgMusicToggle.isOn != musicOn) //if true, music toggle changed since scene load
        {
            if (bgMusicToggle.isOn)
            {
                PlayerPrefs.SetInt("bgmusic", 1);
                gameController.changeBGMusic();
            }
            else
            {
                PlayerPrefs.SetInt("bgmusic", -1);
                gameController.bgMusicSource.Stop();
            }

            musicOn = bgMusicToggle.isOn;
            gameController.bgMusicOn = musicOn;
        }

        //if true, toggle changed from scene load
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
            gameController.sfxOn = soundOn;
            
        }
     }

}
