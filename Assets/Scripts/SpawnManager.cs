using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;
    public float spawnInterval;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Spawn(HPManager toBeSpawned)
    {
        SpawnNode[] nodes = SpawnNode.spawnNodes.Where(node => node.IsSpawnViable()).ToArray();
        if (nodes.Length > 0)
        {
            
            nodes[Random.Range(0,nodes.Length)].Spawn(toBeSpawned);
        }
        else
        {
            SpawnNode.spawnNodes.OrderBy(node => -node.DistanceToPlayer()).FirstOrDefault().Spawn(toBeSpawned);
        }
    }
    public IEnumerator WaitForSpawn(HPManager toBeSpawned)
    {
        yield return new WaitForSeconds(spawnInterval);
        Spawn(toBeSpawned);
    }
}
