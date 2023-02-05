using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string mainSceneName = "Main";
    public Transform creditos;

    void Start()
    {
        
    }

    void Update()
    {
    if(Input.GetKeyDown(KeyCode.Escape)){

        HideCredits();
    }

    }

    public void ShowCredits()
    {
        creditos.gameObject.SetActive(true);
    }

    public void ShowOptions()
    {

    }

    public void HideCredits()
    {
        creditos.gameObject.SetActive(false);
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
