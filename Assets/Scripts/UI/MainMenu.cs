using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnButtonClickJugarMenu ( )
    {
        print ("Jugar");
        SceneManager.LoadScene ("Lobby");
    }

    public void OnButtonClickPerfilesMenu ( )
    {
        SceneManager.LoadScene ("Profiles");
    }

    public void OnButtonClickSalirMenu ( )
    {
        Application.Quit ();
    }

    public void OnButtonClickLogInMenu ( )
    {
        SceneManager.LoadScene ("LogIn");
    }

    public void OnButtonClickRegisterMenu ()
    {
        SceneManager.LoadScene ("Register");
    }

    public void OnButtonClickLogInLogIn ()
    {
        SceneManager.LoadScene ("Menu");
    }

    public void OnButtonClickRegisterRegister ()
    {
        SceneManager.LoadScene ("Menu");
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
