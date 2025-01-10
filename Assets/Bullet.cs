using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 25f; // Daño que inflige la bala

    void Update ()
    {
        transform.Translate (Vector3.forward * speed * Time.deltaTime); // Movimiento recto
    }
}
