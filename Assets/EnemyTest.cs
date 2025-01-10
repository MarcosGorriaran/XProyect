    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EnemyTest : MonoBehaviour
    {
        public float life = 100f;
        public float damage = 0f;
        public Renderer player;
    public Color originalColor;

    // Start is called before the first frame update
    void Start ()
        {
        }

        // Update is called once per frame
        void Update ()
        {
        }

        public void TakeDamage (float damage)
        {
            Debug.Log ("Daño recibido: " + damage); // Muestra un mensaje en consola
            life -= damage;
            if (life <= 0)
            {
                Destroy (gameObject); // Destruye al enemigo si la vida llega a 0
            }
        }

        void OnTriggerEnter (Collider other)
        {
            if (other.CompareTag ("Ball")) // Si colisiona con una bala
            {
            StartCoroutine (ChangeColorTemporarily ());
            TakeDamage (10); // Aplica daño al enemigo
                Debug.Log ("Daño recibido"); // Muestra un mensaje en consola
            }
        
        }

    IEnumerator ChangeColorTemporarily ()
    {
        MeshRenderer meshRenderer = player.GetComponent<MeshRenderer> ();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.red; // Cambia el color a rojo
            yield return new WaitForSeconds (0.2f); // Espera 2 segundos
            meshRenderer.material.color = originalColor; // Restaura el color original
        }
    }
}
