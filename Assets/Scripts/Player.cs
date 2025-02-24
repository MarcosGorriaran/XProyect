using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HPManager _hPManager;
    public PlayerInfo playerInfo;

    private void Awake()
    {
        _hPManager = GetComponent<HPManager>();
        _hPManager.onDeath += OnDeath;
        _hPManager.onDeathBy += OnDeathBy;
        _hPManager.onRevive += OnRevive;
    }

    private void _hPManager_onDeathBy(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    void OnDeath()
    {
        GetComponentsInChildren<Collider>().ToList().ForEach(col=>col.enabled=false);
        GetComponent<Rigidbody>().useGravity = false;
        SpawnManager.instance.StartCoroutine(SpawnManager.instance.WaitForSpawn(_hPManager));
        playerInfo.AddDeath();
    }
    void OnRevive(GameObject gameObject)
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponentsInChildren<Collider>().ToList().ForEach(col => col.enabled = true);
    }

    void OnDeathBy(GameObject killer)
    {
        Debug.Log($"{gameObject.name} ha sido eliminado por {killer.name}.");
        if (killer == null)
        {
            Debug.Log("No hay atacante.");
            return; // Si no hay atacante, salir
        }
        Player killerPlayer = killer.GetComponent<Player>();
        if (killerPlayer != null)
        {
            killerPlayer.playerInfo.AddKill(); // Sumar la kill al atacante
            Debug.Log($"{killer.name} ha eliminado a {gameObject.name}.");
        }
    }
}
