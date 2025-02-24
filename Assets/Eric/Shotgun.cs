using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shotgun : MonoBehaviour, IWeapon
{
    public Transform[] shootSpawns = new Transform[9]; // 9 puntos de disparo (cañones de la escopeta)
    public GameObject bulletPrefab; // Prefab de la bala
    public Image delayAttack; // Barra de recarga

    public float autoRechargeTime = 1f; // Tiempo de recarga
    public int maxBullets = 10; // Máximo de balas en el pool
    public int pelletsPerShot = 9; // Cantidad de perdigones por disparo
    public float bulletSpeed = 15f; // Velocidad de las balas

    private Stack<GameObject> bullets;
    private bool shooting = true;
    private bool duringRecharge = false;
    private bool isRecharged = true;
    private float time = 0f;
    [SerializeField] WeaponSO weaponSO;

    void Start()
    {
        bullets = new Stack<GameObject>();
        for (int i = 0; i < maxBullets; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<BulletController>().weapon = this;
            bullet.SetActive(false);
            bullets.Push(bullet);
        }
    }

    public bool IsRecharged
    {
        get { return isRecharged; }
        set { isRecharged = value; }
    }

    public void Shoot()
    {
        if (bullets.Count < pelletsPerShot)
        {
            Debug.Log("No hay suficientes balas para disparar.");
            return;
        }

        Camera playerCamera = GetComponentInParent<PlayerController>().GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("No se encontró la cámara del jugador.");
            return;
        }

        GameObject shooter = GetComponentInParent<Player>().gameObject; 
        if (shooter == null)
        {
            Debug.LogError("No se encontró el jugador que disparo.");
        }

        // Obtenemos el centro de la pantalla
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint = Physics.Raycast(ray, out hit) ? hit.point : ray.GetPoint(1000);

        // Disparar los perdigones
        for (int i = 0; i < pelletsPerShot; i++)
        {
            if (bullets.Count == 0) break;

            GameObject bullet = bullets.Pop();
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bullet.GetComponent<BulletController>().weapon = this;
            bulletController.shooter = shooter;

            bullet.transform.position = shootSpawns[i].position; // Usamos cada punto de disparo
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Vector3 direction = (targetPoint - shootSpawns[i].position).normalized; // Dirección desde cada punto de disparo
            rb.velocity = direction * bulletSpeed; // Establecemos la velocidad en la dirección calculada
        }

        shooting = false;
        if (delayAttack != null)
        {
            delayAttack.fillAmount = 0;
        }
    }

    public void Attack()
    {
        // Solo se puede disparar si el arma está recargada y no está en proceso de recarga
        if (!isRecharged || duringRecharge)
        {
            Debug.Log("El arma no está recargada o está recargando.");
            return;
        }

        Shoot();
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
        // Marca que está recargando
        duringRecharge = true;
        time = 0f;

        // Proceso de recarga
        while (time < autoRechargeTime)
        {
            time += Time.deltaTime;
            if (delayAttack != null)
            {
                delayAttack.fillAmount = time / autoRechargeTime; // Actualiza la barra de recarga
            }
            yield return null;
        }

        // Añade balas al pool, si es necesario
        if (bullets.Count < maxBullets)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullets.Push(bullet);
        }

        shooting = true;  // El arma ahora puede disparar
        duringRecharge = false;  // Termina la recarga
        isRecharged = true;  // El arma está recargada

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

    public void SetDelayAttackImage(Image image)
    {
        delayAttack = image;
        delayAttack.fillAmount = 1f;
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

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }
}
