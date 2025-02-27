using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;


public class PlayerSelector : MonoBehaviour
{
    private PlayerInput playerInput;
    public int playerIndex;
    public Button[] characterButtons; // P�blico para inspecci�n
    private int currentSelection = 0;
    public GameObject[] playerPrefabs;  // Array de los prefabs de los jugadores
    public Transform instancePosition;  // Donde instanciar los prefabs

    private GameObject currentPlayerPrefab;  // Referencia al prefab instanciado

    private bool isReady = false; // Variable para saber si el jugador est� listo
    private bool canNavigate = true; // Habilita o deshabilita la navegaci�n horizontal

    private SkinPickerManager skinPickerManager; // Referencia al SkinPickerManager

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        skinPickerManager = FindObjectOfType<SkinPickerManager>();

        string instancePositionTag = $"instancePosition{playerIndex}";
        GameObject instanceObj = GameObject.FindGameObjectsWithTag(instancePositionTag).FirstOrDefault();

        if (instanceObj != null)
        {
            Debug.Log("instance position founded");
            instancePosition = instanceObj.transform;
        }
        else
        {
            Debug.LogError($"No se encontr� un objeto con el tag {instancePositionTag}.");
        }

        string buttonGroupTag = $"PlayerButtons{playerIndex}";

        characterButtons = GameObject.FindGameObjectsWithTag(buttonGroupTag)
            .Select(obj => obj.GetComponent<Button>())
            .Where(button => button != null)
            .ToArray();

        if (characterButtons == null || characterButtons.Length == 0)
        {
            Debug.LogError($"No se encontraron botones para Player {playerIndex}");
        }
        else
        {
            Debug.Log($"Player {playerIndex} tiene {characterButtons.Length} botones.");
            UpdateHighlight(0);
        }
    }

    public void Update()
    {
        CheckNewButtons();
    }

    private void CheckNewButtons()
    {
        string buttonGroupTag = $"PlayerButtons{playerIndex}";
        
        Button[] newButtons = GameObject.FindGameObjectsWithTag(buttonGroupTag)
            .Select(obj => obj.GetComponent<Button>())
            .Where(button => button != null)
            .ToArray();
    }


    private void UpdateHighlight(int newSelection)
    {
        if (characterButtons == null || characterButtons.Length == 0)
            return;

        Debug.Log($"Player {playerIndex} cambia el highlight al bot�n {newSelection}.");

        foreach (var button in characterButtons)
        {
            button.GetComponent<Image>().color = Color.white;
        }

        characterButtons[newSelection].GetComponent<Image>().color = Color.yellow;
        currentSelection = newSelection;

        UpdatePlayerPrefab(currentSelection);
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (characterButtons == null || characterButtons.Length == 0)
        {
            Debug.LogError($"Player {playerIndex} no tiene botones configurados.");
            return;
        }

        if (isReady || !canNavigate)
            return;

        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log($"Player {playerIndex} ha navegado con input: {input}.");

        if (input.x > 0)
        {
            // Mover a la derecha
            int nextSelection = (currentSelection + 1) % characterButtons.Length;
            Debug.Log($"Player {playerIndex} selecciona el siguiente bot�n: {nextSelection}.");
            UpdateHighlight(nextSelection);
        }
        else if (input.x < 0)
        {
            // Mover a la izquierda
            int previousSelection = (currentSelection - 1 + characterButtons.Length) % characterButtons.Length;
            Debug.Log($"Player {playerIndex} selecciona el bot�n anterior: {previousSelection}.");
            UpdateHighlight(previousSelection);
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (characterButtons == null || characterButtons.Length == 0)
        {
            Debug.LogError($"Player {playerIndex} no tiene botones configurados para seleccionar.");
            return;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.Equals("SelectProfile"))
        {
            characterButtons[currentSelection].onClick.Invoke();
        }

        if (!isReady)
        {
            Debug.Log($"Player {playerIndex} ha presionado 'Submit' para marcar como listo.");
            MarkAsReady();
            SavePlayerSelection(playerIndex, currentSelection);
            skinPickerManager.SumePlayerReady(playerIndex);
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (characterButtons == null || characterButtons.Length == 0)
        {
            Debug.LogError($"Player {playerIndex} no tiene botones configurados para cancelar.");
            return;
        }

        if (isReady)
        {
            Debug.Log($"Player {playerIndex} ha presionado 'Cancel'. Desmarcando como listo.");
            UnmarkAsReady();
            skinPickerManager.RestPlayerReady(playerIndex);
        }
    }

    public void UpdatePlayerPrefab(int currentSelection)
    {
        if (currentSelection < 0 || currentSelection >= playerPrefabs.Length)
        {
            Debug.LogError($"�ndice de selecci�n inv�lido: {currentSelection}");
            return;
        }

        if (currentPlayerPrefab != null)
        {
            Destroy(currentPlayerPrefab);
        }


        currentPlayerPrefab = Instantiate(playerPrefabs[currentSelection], instancePosition.position, Quaternion.Euler(0, 180, 0));
        Debug.Log($"Instanciado el prefab {playerPrefabs[currentSelection].name} en la posici�n {instancePosition.position}");
    }

    public void MarkAsReady()
    {
        isReady = true;
        canNavigate = false;
        Debug.Log($"Player {playerIndex} est� listo.");
    }

    public void UnmarkAsReady()
    {
        isReady = false;
        canNavigate = true;
        Debug.Log($"Player {playerIndex} ya no est� listo.");
    }

    public void SavePlayerSelection(int playerIndex, int selectedSkin)
    {
        var deviceId = (int)playerInput.devices[0].deviceId; // Guardamos el DeviceID del primer dispositivo
        PlayerPrefs.SetInt($"Player{playerIndex}Skin", selectedSkin);
        PlayerPrefs.SetInt($"Player{playerIndex}DeviceID", deviceId); // Guardamos el DeviceID
        PlayerPrefs.Save();

        Debug.Log($"Guardado Player {playerIndex}: Skin {selectedSkin}, DeviceID {deviceId}");
    }

}

