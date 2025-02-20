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
            PlayerController controller = playerInstance.GetComponentInChildren<PlayerController>();
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
        float defaultAspect = 16f / 9f; // Aspect ratio estándar
        float newAspect;

        for (int i = 0; i < maxPlayers; i++)
        {
            // Crear capas para el jugador y su primera persona
            string playerLayerName = "Player" + i;
            string fpLayerName = "FP" + i;

            int playerLayer = LayerMask.NameToLayer(playerLayerName);
            int fpLayer = LayerMask.NameToLayer(fpLayerName);

            if (playerLayer == -1 || fpLayer == -1)
            {
                Debug.LogError($"Las capas no existen: {playerLayerName} o {fpLayerName}");
                continue;
            }

            // Asignar capas
            instantiatedPlayers[i].layer = playerLayer;
            SetLayerRecursively(instantiatedPlayers[i], playerLayer, fpLayer);

            // Configurar la cámara del jugador
            Camera playerCamera = instantiatedPlayers[i].GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                // No renderizar el propio modelo en tercera persona
                playerCamera.cullingMask &= ~(1 << playerLayer);

                // No renderizar la primera persona de los demás
                for (int j = 0; j < maxPlayers; j++)
                {
                    if (i != j)
                    {
                        int otherFPLayer = LayerMask.NameToLayer("FP" + j);
                        playerCamera.cullingMask &= ~(1 << otherFPLayer);
                    }
                }

                // Renderizar su propia vista en primera persona (brazos/arma)
                playerCamera.cullingMask |= (1 << fpLayer);
            }
        }

        // Configuración de división de pantalla (igual que antes)
        switch (maxPlayers)
        {
            case 2:
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 0.5f);
                newAspect = defaultAspect * 2;
                break;

            case 3:
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                instantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
                newAspect = defaultAspect * 2;
                break;

            case 4:
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                instantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
                instantiatedPlayers[3].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                newAspect = defaultAspect;
                break;

            default:
                return;
        }

        foreach (var player in instantiatedPlayers)
        {
            if (player != null)
            {
                Camera cam = player.GetComponentInChildren<Camera>();
                cam.aspect = newAspect;
                cam.fieldOfView = 60;
            }
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer, int fpLayer)
    {
        // Comprobar si este objeto o alguno de sus ancestros es "FP"
        Transform current = obj.transform;
        bool isInFP = false;

        while (current != null)
        {
            if (current.name == "FP")
            {
                isInFP = true;
                break;
            }
            current = current.parent;
        }

        // Si el objeto pertenece a FP (o es FP), aplicamos la capa fpLayer
        if (isInFP)
        {
            obj.layer = fpLayer;
        }
        else if (obj.GetComponent<Canvas>() == null) // Evitar cambiar la capa del Canvas
        {
            obj.layer = newLayer;
        }

        // Aplicar la función a todos los hijos
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer, fpLayer);
        }
    }
}
