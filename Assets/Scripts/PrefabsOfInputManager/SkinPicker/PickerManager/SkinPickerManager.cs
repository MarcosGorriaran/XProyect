using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkinPickerManager : MonoBehaviour
{
    private int playersReady = 0;
    private int maxPlayers;
    private PlayerInputManager playerInputManager;

    private void Start()
    {
        maxPlayers = PlayerPrefs.GetInt("SelectedNumber");
        playerInputManager = FindAnyObjectByType<PlayerInputManager>();

        if (playerInputManager != null)
        {
            Debug.Log("PlayerInputManager encontrado.");
        }
        else
        {
            Debug.LogError("No se encontr� un PlayerInputManager.");
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Comprueba si hay espacio para m�s jugadores
        if (playerInputManager.playerCount > maxPlayers)
        {
            Debug.Log($"Se alcanz� el l�mite de jugadores ({maxPlayers}). Desconectando jugador {playerInput.playerIndex}.");
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
            Debug.Log("No se puede sumar m�s jugadores listos. M�ximo permitido alcanzado.");
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
            StartGame();
        }
        else
        {
            Debug.Log($"A�n faltan jugadores para estar listos. Jugadores listos: {playersReady}");
        }
    }

    private void StartGame()
    {
        if (AcountManager.Session != null)
        {
            SceneManager.LoadScene("SelectProfile");
        }
        else
        {
            SceneManager.LoadScene("Scenary");
        }
        
    }
}
