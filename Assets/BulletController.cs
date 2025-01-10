using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody bulletRB;
    public float power = 100f;
    public float lifeTime = 400f;

    private float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody> ();

        bulletRB.velocity = this.transform.forward * power;
    }

    private void FixedUpdate ()
    {
        time += Time.deltaTime;
        if (time >= lifeTime)
        {
            Destroy (this.gameObject);
        }
    }

}
