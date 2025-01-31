using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Crossbow : MonoBehaviour, IWeapon
{
    public Transform shootSpawn; // Sitio de spawn de balas
    public GameObject bulletPrefab; // Prefab de la bala
    private Image delayAttack; // Barra de recarga
    public float autoRechargeTime = 1f; // Tiempo de recarga por bala
    public int maxBullets = 10; // Máximo de balas en el pool
    public bool shooting = true; // Permite disparar
    public bool duringRecharge = false;
    private Image _rechargeTime;
    private Stack<GameObject> bullets; // Pool de balas
    private float time = 0f;

    void Start()
    {
        bullets = new Stack<GameObject>();
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<BulletController>().weapon = this; // Asigna el arma a la bala
            bullet.SetActive(false);
            bullets.Push(bullet);
        }
    }

    public void InstantieateBullet()
    {
        if (bullets.Count > 0)
        {
            Shoot();
        }
        else
        {
            Debug.Log ("No hay balas disponibles. Recarga o espera...");
        }
    }

    public void Shoot()
    {
        if (bullets.Count > 0)
        {
            GameObject bullet = bullets.Pop();
            bullet.GetComponent<BulletController>().weapon = this; // Asigna el arma a la bala
            bullet.transform.position = shootSpawn.position;
            bullet.transform.rotation = shootSpawn.rotation;
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = shootSpawn.forward * 10;

            duringRecharge = false;
        }
        else
        {
            Debug.Log("No hay balas disponibles para disparar.");
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

    private IEnumerator RechargeBullets()
    {

        if (bullets.Count < maxBullets)
        {
            Debug.Log ("Recargando bala...");
            duringRecharge = true;
            time = 0f; // Reinicia el progreso del tiempo de recarga

            while (time < autoRechargeTime)
            {
                time += Time.deltaTime;
                delayAttack.fillAmount = time / autoRechargeTime; // Actualiza la barra de recarga
                yield return null; // Espera al siguiente frame
            }

            // Crear o reusar bala
            GameObject bullet = bullets.Count < maxBullets ? Instantiate(bulletPrefab) : null;
            if (bullet != null)
            {
                bullet.SetActive(false);
                bullets.Push(bullet);
                Debug.Log("Bala recargada.");
            }
        }
        else
        {
            Debug.Log ("El pool ya está lleno. No se pueden añadir más balas.");
        }

        shooting = true;
        duringRecharge = false;
        delayAttack.fillAmount = 1f; // Reinicia la barra al finalizar
    }

    public void DisactiveBullet(GameObject bullet)
    {
        bullet.SetActive (false);
        if (!bullets.Contains (bullet))
        {
            bullets.Push (bullet);
        }
    }

    public int GetPoolCount()
    {
        return bullets.Count;
    }

    public void SetDelayAttackImage(Image image)
    {
        delayAttack = image;
        delayAttack.fillAmount = 1f; 
    }

}
