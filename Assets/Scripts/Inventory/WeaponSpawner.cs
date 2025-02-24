using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private GameObject[] weapons;
    private GameObject currentWeapon;

    private void Start()
    {
        // Obtener todas las armas que están dentro del spawnPoint
        weapons = new GameObject[spawnPoint.childCount];
        for (int i = 0; i < spawnPoint.childCount; i++)
        {
            weapons[i] = spawnPoint.GetChild(i).gameObject;
            weapons[i].SetActive(false); // Asegurarse de que todas están desactivadas al inicio
        }

        StartCoroutine(SpawnWeapon());
    }

    private IEnumerator SpawnWeapon()
    {
        while (true)
        {
            if (currentWeapon != null)
            {
                currentWeapon.SetActive(false);
            }

            int randomWeaponIndex = Random.Range(0, weapons.Length);
            currentWeapon = weapons[randomWeaponIndex];
            currentWeapon.SetActive(true);

            yield return new WaitForSeconds(10f);
        }
    }
}
