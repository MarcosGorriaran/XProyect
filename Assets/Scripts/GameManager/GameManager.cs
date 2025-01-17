using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject[] playerSpawners;
    int selectedNumber;
    private GameObject[] InstantiatedPlayers;

    private void Start()
    {
        InstanciatePlayers();
    }

    private void InstanciatePlayers()
    {
        // Recupera el número desde PlayerPrefs
        selectedNumber = PlayerPrefs.GetInt("SelectedNumber");
        InstantiatedPlayers = new GameObject[selectedNumber];
        for (int i = 0; i < selectedNumber; i++)
        {
            GameObject player = Instantiate(playerPrefabs[i], playerSpawners[i].transform.position, Quaternion.identity);
            InstantiatedPlayers[i] = player;
        }
        ScreenDivision();
    }

    private void ScreenDivision()
    {
        switch (selectedNumber)
        {
            case 2:
                // Dividir en dos cámaras horizontales
                InstantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f); // Parte superior
                InstantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 0.5f);    // Parte inferior
                break;

            case 3:
                // Dividir en 4 secciones, dejando la última sección vacía
                InstantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f); // Superior izquierda
                InstantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Superior derecha
                InstantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f); // Inferior izquierda
                break;

            case 4:
                // Dividir en 4 secciones iguales
                InstantiatedPlayers[0].GetComponentInChildren<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f); // Superior izquierda
                InstantiatedPlayers[1].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f); // Superior derecha
                InstantiatedPlayers[2].GetComponentInChildren<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f); // Inferior izquierda
                InstantiatedPlayers[3].GetComponentInChildren<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f); // Inferior derecha
                break;

            default:
                Debug.LogWarning("ScreenDivision no está configurado para " + selectedNumber + " jugadores.");
                break;
        }
    }
}
