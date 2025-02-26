using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SelectProfileManager : PickManager
{
    private int maxPlayers;
    private int playersReady;
    private PlayerInputManager playerInputManager;
    // Start is called before the first frame update
    

    protected override void NextScene()
    {
        SceneManager.LoadScene("Scenary");
    }
}
