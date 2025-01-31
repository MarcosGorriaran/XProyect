using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    public static List<SpawnNode> spawnNodes { private set; get; } = new List<SpawnNode>();
    [SerializeField]
    float playerDetectionRadius;
    public bool IsSpawnViable()
    {
        Collider[] colisions = Physics.OverlapSphere(transform.position, playerDetectionRadius);
        return colisions.Where(col => col.TryGetComponent<Player>(out _) && !col.GetComponent<HPManager>().IsDead()).Count() <= 0;
    }
    public float DistanceToPlayer()
    {
        Collider[] colisions = Physics.OverlapSphere(transform.position, playerDetectionRadius).Where(col => col.TryGetComponent<Player>(out _) && !col.GetComponent<HPManager>().IsDead()).ToArray();
        if (colisions.Length <= 0)
        {
            return float.MaxValue;
        }
        return colisions.Min(col => Vector3.Distance(col.transform.position, transform.position));
    }
    public void Spawn(HPManager toBeSpawned)
    {
        toBeSpawned.transform.position = transform.position;
        toBeSpawned.Revive();
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
    private void OnEnable()
    {
        spawnNodes.Add(this);
    }
    private void OnDisable()
    {
        spawnNodes.Remove(this);
    }
}
