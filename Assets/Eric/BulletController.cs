using UnityEngine;

public class BulletController : MonoBehaviour
{
    public IWeapon weapon;
    public GameObject shooter;// <-- Jugador que disparó la bala

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
        {
            if (weapon != null)
            {
                weapon.DisactiveBullet(gameObject);
            }
            Debug.Log("Bala impactó contra un obstáculo/enemigo y fue devuelta.");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            HPManager enemyHP = collision.gameObject.GetComponent<HPManager>();
            if (enemyHP != null)
            {
                float damage = 10;
                enemyHP.Hurt(damage, shooter); // Pasa el atacante como argumento
                Debug.Log("Bala impactó contra un enemigo y le hizo daño.");
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Map") && weapon != null)
        {
            weapon.DisactiveBullet(gameObject);
            Debug.Log("Bala salió del mapa y fue devuelta.");
        }

    }
}
