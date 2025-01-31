using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Shotgun : MonoBehaviour , IWeapon
{
    public Transform shootSpawn1;
    public Transform shootSpawn2;
    public Transform shootSpawn3;
    public Transform shootSpawn4;
    public Transform shootSpawn5;

    public GameObject bulletPrefab; // Prefab de la bala
    private Image delayAttack;       // Barra de recarga

    public float autoRechargeTime = 1f; // Tiempo de recarga por bala
    public int maxBullets = 10;         // Máximo de balas en el pool
    public bool shooting = true;       // Permite disparar
    public bool duringRecharge = false;

    private Stack<GameObject> bullets; // Pool de balas
    private float time = 0f;


    void Start ()
    {
        bullets = new Stack<GameObject>();
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<BulletController>().weapon = this; // Asigna el arma a la bala
            bullet.SetActive(false);
            bullets.Push(bullet);
        }
        if (delayAttack != null)
        {
            Debug.Log("Barra de recarga asignada.");
        }
    }

    public void UpdateDelayAttack()
    {
        // Actualiza la barra de recarga
        if (duringRecharge)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
        delayAttack.fillAmount = time / autoRechargeTime;

    }

    public void Shoot()
    {
        Transform[] shootSpawns = { shootSpawn1, shootSpawn2, shootSpawn3, shootSpawn4, shootSpawn5 };

        int bulletsToShoot = Mathf.Min(5, bullets.Count); // Máximo de 5 balas o menos si no hay suficientes

        for (int i = 0; i < bulletsToShoot; i++)
        {
            Transform shootSpawn = shootSpawns[i];
            GameObject bullet = bullets.Pop();
            bullet.GetComponent<BulletController>().weapon = this; // Asigna el arma a la bala
            bullet.transform.position = shootSpawn.position;
            bullet.transform.rotation = shootSpawn.rotation;
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = shootSpawn.forward * 10;
        }

        if (bullets.Count == 0)
        {
            Debug.Log("No hay más balas en el pool.");
        }
    }


    public void InstantieateBullet ()
    {
        if (bullets.Count >= 5)
        {
            Shoot ();
        }
        else
        {
            Debug.Log ("No hay suficientes balas disponibles. Recarga o espera...");
        }
    }

    public void Attack()
    {
        if (!shooting || delayAttack == null || delayAttack.fillAmount == 0)
        {
            Debug.Log("El arma está recargando. No puedes disparar.");
            return;
        }

        InstantieateBullet();
        shooting = false;
        delayAttack.fillAmount = 0;
    }

    public void Recharge()
    {
        if (!duringRecharge)
        {
            StartCoroutine(RechargeBullets());
        }
    }

    public void DisactiveBullet (GameObject bullet)
    {
        bullet.SetActive (false);
        if (!bullets.Contains (bullet)) // Evita duplicados en el pool
        {
            bullets.Push (bullet);
        }
    }

    private IEnumerator RechargeBullets()
    {
        Debug.Log("Recargando balas...");
        duringRecharge = true;

        for (int i = 0; i < 5; i++) // Recarga hasta 5 balas
        {
            if (bullets.Count < maxBullets)
            {
                yield return new WaitForSeconds(autoRechargeTime / 5);

                // Crear una nueva bala si el pool no está lleno
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                bullets.Push(bullet);
            }
            else
            {
                Debug.Log("El pool ya está lleno. No se pueden añadir más balas.");
                break;
            }

            // Actualizar la barra de recarga
            delayAttack.fillAmount = (float)(i + 1) / 5f;
        }

        shooting = true;
        duringRecharge = false;
        Debug.Log("Recarga completa: 5 balas añadidas.");
    }




    public int GetPoolCount ()
    {
        return bullets.Count;
    }

    public void SetDelayAttackImage(Image image)
    {
        delayAttack = image;
        delayAttack.fillAmount = 1f; 
    }

}
