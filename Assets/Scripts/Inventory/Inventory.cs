using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private WeaponSO[] slots;

    private IWeapon activeWeapon; // Referencia al arma activa
    private Transform weaponHolder;

    private void Awake()
    {
        weaponHolder = transform.Find("WeaponHolder");
        if (weaponHolder == null)
        {
            weaponHolder = new GameObject("WeaponHolder").transform;
            weaponHolder.SetParent(transform);
        }
    }

    public IWeapon ChangeWeapon(uint slotIndex)
    {
        if (slotIndex >= slots.Length || slots[slotIndex] == null)
        {
            return null;
        }

        // Destruir el arma actual
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        // Instanciar la nueva arma
        GameObject weaponInstance = Instantiate(slots[slotIndex].weaponPrefab, weaponHolder);
        IWeapon weaponScript = weaponInstance.GetComponent<IWeapon>();

        if (weaponScript != null)
        {
            activeWeapon = weaponScript;
            return weaponScript;
        }

        Debug.LogError("El prefab del arma no tiene un componente que implemente IWeapon.");
        return null;
    }

    public IWeapon GetActiveWeapon()
    {
        return activeWeapon;
    }

    public int GetInventorySize()
    {
        return slots.Count(weapon => weapon != null);
    }
}
