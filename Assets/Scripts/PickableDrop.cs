using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickableDrop : MonoBehaviour
{
    public WeaponSO weapon;
    private GameObject objectModel;
    [SerializeField]
    private float waitTime;
    private void Awake()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Inventory inventory))
        {
            inventory.AddWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
