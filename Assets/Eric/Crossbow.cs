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
    private bool isRecharged = true;
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

    public bool IsRecharged
    {
        get { return isRecharged; }
        set { isRecharged = value; }
    }

    public void InstantieateBullet()
    {
        if (bullets.Count > 0)
        {
            Shoot();
        }
        else
        {
            Debug.Log("No hay balas disponibles. Recarga o espera...");
        }
    }

    public void Shoot()
    {
        // Solo se puede disparar si el arma está recargada y no está en proceso de recarga
        if (!isRecharged || duringRecharge)
        {
            Debug.Log("El arma no está recargada o está recargando.");
            return;
        }

        GameObject bullet = bullets.Pop();
        bullet.GetComponent<BulletController>().weapon = this; // Asigna el arma a la bala
        bullet.transform.position = shootSpawn.position;
        bullet.transform.rotation = shootSpawn.rotation;
        bullet.SetActive(true);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        Camera playerCamera = GetComponentInParent<PlayerController>().GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("No se encontró la cámara del jugador.");
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Centro de la pantalla
        RaycastHit hit;

        Vector3 targetPoint;

        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 2f);

        if (Physics.Raycast(ray, out hit)) // Si golpea algo
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(1000); // Si no golpea nada, apunta lejos (1000 unidades adelante)
        }

        Vector3 direction = (targetPoint - shootSpawn.position).normalized;

        rb.velocity = direction * 20f; // Ajusta la velocidad de la bala

        // Marca el arma como no recargada después de disparar
        isRecharged = false;

        Debug.Log("Disparo realizado.");
    }


    public void Attack()
    {
        // Solo se puede disparar si el arma está recargada y no está en proceso de recarga
        if (!isRecharged || duringRecharge)
        {
            Debug.Log("El arma no está recargada o está recargando.");
            return;
        }

        InstantieateBullet();
        isRecharged = false;  // El arma ya no está recargada después de disparar
        Debug.Log("Disparo realizado.");
    }

    public void Recharge()
    {
        // Solo se puede recargar si no está en proceso de recarga
        if (duringRecharge)
        {
            Debug.Log("El arma ya está recargando.");
            return;
        }

        // Inicia la recarga
        StartCoroutine(RechargeBullets());
        isRecharged = false;  // Aseguramos que al empezar la recarga, el arma no esté recargada
        Debug.Log("Recargando el arma...");
    }

    private IEnumerator RechargeBullets()
    {
        duringRecharge = true;
        time = 0f;

        while (time < autoRechargeTime)
        {
            time += Time.deltaTime;
            if (delayAttack != null)
            {
                delayAttack.fillAmount = time / autoRechargeTime;
            }
            yield return null;
        }

        // Después de la recarga, asegúrate de que el arma esté lista para disparar
        isRecharged = true;  // Ahora el arma está recargada

        shooting = true; // Permite que el arma dispare
        duringRecharge = false; // Marca que la recarga ha terminado

        // Restaura la barra de recarga a su valor máximo
        if (delayAttack != null)
        {
            delayAttack.fillAmount = 1f;
        }

        Debug.Log("Recarga completada.");
    }

    public void DisactiveBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        if (!bullets.Contains(bullet))
        {
            bullets.Push(bullet);
        }
    }

    public int GetPoolCount()
    {
        return bullets.Count;
    }

    public void SetPoolCount(int count)
    {
        while (bullets.Count > count)
        {
            bullets.Pop();
        }
    }

    public void SetDelayAttackImage(Image image)
    {
        delayAttack = image;
        delayAttack.fillAmount = 1f;
    }
}
