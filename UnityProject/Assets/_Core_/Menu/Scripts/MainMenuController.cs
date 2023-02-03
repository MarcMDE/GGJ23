using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string mainSceneName = "Main";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowCredits()
    {

    }

    public void ShowOptions()
    {

    }

    public void HideCredits()
    {

    }

    public void HideOptions()
    {

    }

    public void Play()
    {
        Debug.Log("Play game...");
        SceneManager.LoadScene(mainSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exit game...");
        Application.Quit();
    }
}
