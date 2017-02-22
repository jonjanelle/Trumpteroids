using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour {

    public Button button;


    void Start()
    {
        if (button.tag.Equals("StartButton")){
            button.onClick.AddListener(StartClicked);
        }
        else if (button.tag.Equals("QuitButton"))
        {
            button.onClick.AddListener(QuitClicked);
        }
        else if (button.tag.Equals("OptionsButton"))
        {
            button.onClick.AddListener(OptionsClicked);
        }
        else if (button.tag.Equals("MainMenuButton"))
        {
            button.onClick.AddListener(MainMenuClicked);
        }
        else if (button.tag.Equals("PauseButton"))
        {
            button.onClick.AddListener(PauseClicked);
        }
        else if (button.tag.Equals("RestartButton"))
        {
            button.onClick.AddListener(StartClicked);
        }
        else if (button.tag.Equals("ResumeButton"))
        {
            button.onClick.AddListener(ResumeClicked);
        }

    }

    void StartClicked()
    {
        
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync("options 1");
            SceneManager.UnloadSceneAsync("scene");
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("scene");
    }

    void QuitClicked()
    {
        Application.Quit();
    }

    void OptionsClicked()
    {
        SceneManager.LoadScene("options");
    }

    void MainMenuClicked()
    {
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync("options 1");
            SceneManager.UnloadSceneAsync("scene");
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("menu");
    }

    void PauseClicked()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("options 1", LoadSceneMode.Additive);
        //SceneManager.LoadScene("options 1");
    }
    void ResumeClicked()
    {
        SceneManager.UnloadSceneAsync("options 1");
        Time.timeScale = 1;
    }
}
