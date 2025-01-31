using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    private GameObject[] instantiatedPlayers; //llista de los jugadores instanciados para controlar sus camaras 

    public int maxPlayers = 0;


    private void Start()
    {
        maxPlayers = PlayerPrefs.GetInt("SelectedNumber");
        instantiatedPlayers = new GameObject[maxPlayers];
        StartCoroutine(AssignPlayers());
    }

    private IEnumerator AssignPlayers()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            int skin = PlayerPrefs.GetInt($"Player{i}Skin", -1);
            int deviceId = PlayerPrefs.GetInt($"Player{i}DeviceID", -1);

            if (skin == -1 || deviceId == -1)
            {
                Debug.LogWarning($"No se encontr? configuraci?n para Player {i}");
                continue;
            }

            //Instanciar el player amb la skin

            GameObject playerInstance = Instantiate(playerPrefabs[skin], spawnPoints[i].position, spawnPoints[i].rotation);
            instantiatedPlayers[i] = playerInstance;
            PlayerController controller = playerInstance.GetComponent<PlayerController>();
            controller.SetPlayerIndex(i);
            Debug.Log($"Esperando input para Player {i} con DeviceID {deviceId}...");

            bool assigned = false;

            // Esperar hasta que el PlayerInput est? completamente inicializado
            PlayerInput playerInput = FindObjectOfType<PlayerInput>();
            while (playerInput == null || !playerInput.enabled) // Espera hasta que PlayerInput est? listo
            {
                yield return null;
                playerInput = FindObjectOfType<PlayerInput>(); ; // Intentar obtener PlayerInput nuevamente
            }

            while (!assigned)
            {
                Debug.Log($"Buscando dispositivo con DeviceID {deviceId}...");
                // Escucha los dispositivos conectados
                foreach (var device in InputSystem.devices)
                {
                    Debug.Log($"Dispositivo: {device.name}, DeviceID: {(int)device.deviceId}");
                    if ((int)device.deviceId == deviceId)
                    {
                        playerInput.SwitchCurrentControlScheme(device);
                        playerInput.user.AssociateActionsWithUser(playerInput.actions);
                        Debug.Log($"Player {i} asignado al dispositivo {device.name} con DeviceID {deviceId}");
                        assigned = true;
                        break;
                    }
                }

                yield return null; // Espera un frame antes de volver a comprobar
            }
        }
        ScreenDivision();
    }
    private void ScreenDivision()
    {
        // Dependiendo del n?mero de jugadores, divide las c?maras
        switch (maxPlayers)
        {
            case 2:
                // Para 2 jugadores, dividir la pantalla en dos partes (arriba y abajo)
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f); // Arriba
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 0.5f);   // Abajo
                break;

            case 3:
                // Para 3 jugadores, pantalla dividida como si fuera entre 4, pero la parte inferior derecha vac?a
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f); // Arriba izquierda
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Arriba derecha
                instantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f); // Abajo izquierda
                break;

            case 4:
                // Para 4 jugadores, dividir la pantalla en 4 partes iguales
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f); // Arriba izquierda
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Arriba derecha
                instantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f); // Abajo izquierda
                instantiatedPlayers[3].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f); // Abajo derecha
                break;
        }
    }

}
