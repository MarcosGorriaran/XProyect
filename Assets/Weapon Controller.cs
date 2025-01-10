using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform shootSpawn;
    public Transform shootSpawn2;
    public Transform shootSpawn3;
    public Transform shootSpawn4;
    public Transform shootSpawn5;

    public bool shooting;

    public GameObject bulletPrefab;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        shooting = Input.GetKeyDown (KeyCode.Mouse0);

        if (shooting)
        {
            InstantieateBullet ();
        }
    }

    public void InstantieateBullet ()
    {
        Instantiate (bulletPrefab, shootSpawn.position, shootSpawn.rotation);
        Instantiate (bulletPrefab, shootSpawn2.position, shootSpawn2.rotation);
        Instantiate (bulletPrefab, shootSpawn3.position, shootSpawn3.rotation);
        Instantiate (bulletPrefab, shootSpawn4.position, shootSpawn4.rotation);
        Instantiate (bulletPrefab, shootSpawn5.position, shootSpawn5.rotation);
    }
}
