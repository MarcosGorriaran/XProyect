using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android.LowLevel;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] playerPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    private GameObject[] instantiatedPlayers; //llista de los jugadores instanciados para controlar sus camaras 
    public Countdown timer;
    public int maxPlayers = 0;
    public PlayerInfo[] playerInfos;
    public DynamicAudioListener audioListener;

    private void Start()
    {
        maxPlayers = PlayerPrefs.GetInt("SelectedNumber");
        instantiatedPlayers = new GameObject[maxPlayers];
        AssignPlayers();
    }


    private void AssignPlayers()
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
            controller.SetInputIndex(deviceId);
            Debug.Log($"Esperando input para Player {i} con DeviceID {deviceId}...");
        }
        ScreenDivision();
        audioListener.GetPlayers();

    }
    private void ScreenDivision()
    {
        float defaultAspect = 16f / 9f; // Aspect ratio est�ndar
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

            // Configurar la c�mara del jugador
            Camera playerCamera = instantiatedPlayers[i].GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                // No renderizar el propio modelo en tercera persona
                playerCamera.cullingMask &= ~(1 << playerLayer);

                // No renderizar la primera persona de los dem�s
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

        // Configuraci�n de divisi�n de pantalla (igual que antes)
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
                newAspect = defaultAspect;
                ScaleCanvas();
                break;

            case 4:
                instantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                instantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                instantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
                instantiatedPlayers[3].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                newAspect = defaultAspect;
                ScaleCanvas();
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
        timer.isRunning = true;
        PlayerInfos();
}

    private void ScaleCanvas()
    {
        foreach (var player in instantiatedPlayers)
        {
            if (player != null)
            {
                Canvas canvas = player.GetComponentInChildren<Canvas>();
                if (canvas != null)
                {
                    //coger el panel hijo del canvas
                    Debug.LogError("ResizeCanvas");
                    //buscar un panel en el child de canvas
                    Transform panel = canvas.transform.Find("Container");
                    panel.transform.localScale = new Vector3(1f, 2f, 0.5f);

                }
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

        // Aplicar la funci�n a todos los hijos
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer, fpLayer);
        }
    }

    private void PlayerInfos()
    {
        playerInfos = FindObjectsOfType<PlayerInfo>();
    }

    public void CheckWinner()
    {
        if (playerInfos.FirstOrDefault(playerInfo => playerInfo.Winner = true))
        {
            return;
        }
        foreach (PlayerInfo playerInfo in playerInfos)
        {
            if (playerInfo.Kills >= 20)
            {
                timer.isRunning = false;
                playerInfo.Winner = true;
                //string con el nombre del player quitandole la palabra (Clone)
                SetVideo(playerInfo.playerName);
            }
        }
    }

    public void PlayerWithMoreKills()
    {
        PlayerInfo playerWithMoreKills = playerInfos.OrderByDescending(playerInfo => playerInfo.Kills).FirstOrDefault(); // Obtener el jugador con m�s kills
        Debug.Log($"El jugador con m�s kills es el Player {playerWithMoreKills.playerID} con {playerWithMoreKills.Kills} kills.");
        //mirar el nombre del jugador con m�s kills

        playerWithMoreKills.Winner = true;
        SetVideo(playerWithMoreKills.playerName);
    }

    public void SetVideo(string videoName)
    {
        PlayerPrefs.SetString("SelectedVideo", videoName);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Cinematic");
    }







}
