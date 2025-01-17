using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private HPManager _hPManager;

    private void Awake()
    {
        _hPManager = GetComponent<HPManager>();
        _hPManager.onDeath += OnDeath;
        _hPManager.onRevive += OnRevive;
    }
    void OnDeath()
    {
        GetComponentsInChildren<Collider>().ToList().ForEach(col=>col.enabled=false);
        GetComponent<Rigidbody>().useGravity = false;
        SpawnManager.instance.StartCoroutine(SpawnManager.instance.WaitForSpawn(_hPManager));
    }
    void OnRevive(GameObject gameObject)
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponentsInChildren<Collider>().ToList().ForEach(col => col.enabled = true);
    }
}
