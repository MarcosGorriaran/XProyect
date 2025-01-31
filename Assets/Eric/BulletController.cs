using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public IWeapon weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && weapon != null)
        {
            weapon.DisactiveBullet(gameObject);
            Debug.Log("Bala impact� al enemigo y fue devuelta.");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Map") && weapon != null)
        {
            weapon.DisactiveBullet(gameObject);
            Debug.Log("Bala sali� del mapa y fue devuelta.");
        }
    }
}
