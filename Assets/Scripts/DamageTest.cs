using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public float damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HPManager hPManager))
        {
            hPManager.Hurt(damage,gameObject);
        }
    }
}
