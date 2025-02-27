using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicAudioListener : MonoBehaviour
{

    [SerializeField] private GameObject[] players;
    private void Update()
    {
        CalculePosition();
    }

    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Enemy"); // Buscar todos los jugadores en la escena
    }

    private void CalculePosition()
    {
        if (players == null || players.Length == 0)
        {
            Debug.LogWarning("No hay jugadores asignados o GetPlayers() no se llamó correctamente.");
            return;
        }

        Vector3 averagePosition = Vector3.zero;

        foreach (GameObject player in players)
        {
            averagePosition += player.transform.position;
        }

        averagePosition /= players.Length; // Calcular punto medio

        // Mover suavemente el AudioListener
        transform.position = Vector3.Lerp(transform.position, averagePosition, Time.deltaTime * 5f);
    }


}
