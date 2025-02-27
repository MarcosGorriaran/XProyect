using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


//Luckily there is only one class it inherits its properties and it is the SelectProfileManager
public abstract class PickManager : MonoBehaviour
{
    private int playersReady = 0;
    protected int maxPlayers;
    private PlayerInputManager playerInputManager;
    private static PickManager _instance;

    public static PickManager Instance
    {
        get { return _instance; }

    }
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    protected virtual void Start()
    {
        maxPlayers = PlayerPrefs.GetInt("SelectedNumber");
        playerInputManager = FindAnyObjectByType<PlayerInputManager>();

        if (playerInputManager != null)
        {
            Debug.Log("PlayerInputManager encontrado.");
        }
        else
        {
            Debug.LogError("No se encontró un PlayerInputManager.");
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Comprueba si hay espacio para más jugadores
        if (playerInputManager.playerCount > maxPlayers)
        {
            Debug.Log($"Se alcanzó el límite de jugadores ({maxPlayers}). Desconectando jugador {playerInput.playerIndex}.");
            Destroy(playerInput.gameObject); // Elimina el jugador adicional
        }
        else
        {
            Debug.Log($"Jugador {playerInput.playerIndex} unido. Total: {playerInputManager.playerCount}/{maxPlayers}");
        }
    }

    public void SumePlayerReady(int playerIndex)
    {
        if (playersReady < maxPlayers)
        {
            playersReady++;
            CheckPlayersReady();
        }
        else
        {
            Debug.Log("No se puede sumar más jugadores listos. Máximo permitido alcanzado.");
        }
    }

    public void RestPlayerReady(int playerIndex)
    {
        if (playersReady > 0)
        {
            playersReady--;
            CheckPlayersReady();
        }
        else
        {
            Debug.Log("No hay jugadores listos para quitar.");
        }
    }

    public void CheckPlayersReady()
    {
        if (playersReady == maxPlayers)
        {
            NextScene();
        }
        else
        {
            Debug.Log($"Aún faltan jugadores para estar listos. Jugadores listos: {playersReady}");
        }
    }

    protected abstract void NextScene();
}
