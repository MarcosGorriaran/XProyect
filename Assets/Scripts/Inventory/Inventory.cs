using System.Linq;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private WeaponSO[] slots = new WeaponSO[2];
    [SerializeField]  private GameObject[] firstPersonWeaponModels;
    private IWeapon activeWeapon; // Referencia al arma activa
    private Transform weaponHolder;
    private Transform headHolder;
    private bool hasGorrocoptero;

    private void Awake()
    {
        Transform rightHand = GetComponentsInChildren<Transform>()
                           .FirstOrDefault(t => t.name == "mixamorig:RightHand");
        Transform head = GetComponentsInChildren<Transform>()
                           .FirstOrDefault(t => t.name == "mixamorig:HeadTop_End");
        if (rightHand != null)
        {
            Debug.Log("mixamorig:RightHand encontrado: " + rightHand.name);
            weaponHolder = rightHand.Find("WeaponHolder");

            if (weaponHolder != null)
            {
                Debug.Log("WeaponHolder encontrado correctamente en " + weaponHolder.name);
            }
            else
            {
                Debug.LogError("Error: No se encontró WeaponHolder dentro de RightHand.");
            }
        }
        else
        {
            Debug.LogError("Error: No se encontró mixamorig:RightHand en la jerarquía.");
        }
        if(head != null)
        {
            headHolder = head.Find("HeadHolder");

            if (headHolder != null)
            {
                Debug.Log("HeadHolder encontrado correctamente en " + headHolder.name);
            }
            else
            {
                Debug.LogError("Error: No se encontró HeadHolder dentro de HeadTop_End.");
            }
        }
        else
        {
            Debug.LogError("Error: No se encontró mixamorig:HeadTop_End en la jerarquía.");
        }
        SetDefaultWeapon("FPTrabuco");

    }

    private void SetDefaultWeapon(string defaultWeaponName)
    {
        WeaponSO defaultWeapon = slots.FirstOrDefault(weapon => weapon.weaponName == defaultWeaponName);
        if(defaultWeapon != null)
        {
            // Obtener el índice del WeaponSO
            int slotIndex = Array.FindIndex(slots, weapon => weapon == defaultWeapon);

            // Verifica que el índice es válido y luego cambia el arma
            if (slotIndex >= 0)
            {
                ChangeWeapon((uint)slotIndex);  // Convertir a uint antes de pasarlo
            }
            else
            {
                Debug.LogWarning($"El WeaponSO '{defaultWeaponName}' no se encuentra en los slots.");
            }
        }
        else
        {
            Debug.LogWarning($"El WeaponSO '{defaultWeaponName}' no se encuentra en los slots.");
        }
    }

    public IWeapon ChangeWeapon(uint slotIndex)
    {
        if (slotIndex >= slots.Length || slots[slotIndex] == null)
        {
            return null;
        }

        // Instancia la nueva arma
        Transform parentHolder = slots[slotIndex].weaponName == "FPGorrocoptero" ? headHolder : weaponHolder;

        // Elimina el arma actual
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in headHolder)
        {
            Destroy(child.gameObject);
        }

        GameObject weaponInstance = Instantiate(slots[slotIndex].weaponPrefab, parentHolder);
        weaponInstance.transform.localPosition = slots[slotIndex].positionOffset;
        weaponInstance.transform.localEulerAngles = slots[slotIndex].rotationOffset;
       
        string weaponName = slots[slotIndex].weaponName;
        foreach (var model in firstPersonWeaponModels)
        {
            model.SetActive(false);
        }
        GameObject firstPersonWeapon = null;
        foreach (var model in firstPersonWeaponModels)
        {
            if (model.name == weaponName)
            {
                model.SetActive(true);
                firstPersonWeapon = model;
                break;
            }
        }
        if(firstPersonWeapon != null)
        {
            IWeapon weaponScript = firstPersonWeapon.GetComponentInChildren<IWeapon>();
            if(weaponScript != null)
            {
                activeWeapon = weaponScript;
            }
            else
            {
                Debug.LogError("El prefab del arma no tiene un componente que implemente IWeapon.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el modelo del arma en firstPersonWeaponModels.");
        }

        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            int playerIndex = playerController.GetPlayerIndex();
            int ThirdPersonLayer = LayerMask.NameToLayer("Player" + playerIndex);

            Camera playerCamera = playerController.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                SetLayerRecursively(weaponInstance, ThirdPersonLayer);
            }
        }
        return activeWeapon;
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public IWeapon GetActiveWeapon()
    {
        return activeWeapon;
    }

    public int GetInventorySize()
    {
        return slots.Count(weapon => weapon != null);
    }

    public GameObject GetActiveFirstPersonWeapon()
    {
        foreach (var model in firstPersonWeaponModels)
        {
            if (model.activeSelf)
            {
                return model;
            }
        }
        return null;
    }

    public void AddWeapon(WeaponSO weapon)
    {
        Debug.Log("Añadiendo arma al inventario: " + weapon.weaponName);
        // Busca el primer slot vacío del inventario
        int emptySlot = Array.FindIndex(slots, slot => slot == null);
        // Si no hay slots vacíos, no se puede añadir el arma
        if (emptySlot == -1)
        {
            Debug.LogWarning("No hay slots vacíos en el inventario.");
            return;
        }
        // Añade el arma al primer slot vacío
        //si el arma que se recoge ya esta en el inventario no se hace nada
        if(slots.Contains(weapon))
        {
            Debug.Log("El arma que se recoge ya está en el inventario.");
            return;
        }
        slots[emptySlot] = weapon;
    }

    public void ChangeWeaponInventory(WeaponSO newWeapon)
    {
        foreach (var WeaponModel in firstPersonWeaponModels)
        {
            if (WeaponModel.activeSelf) 
            {
                WeaponSO weaponActive = slots.FirstOrDefault(weapon => weapon.weaponName == WeaponModel.name); // Busca el arma actual en el inventario
                //si el arma activa es la misma que la que se recoge, no se hace nada o si la que se recoge ya esta en el inventario no se hace nada
                if(weaponActive == newWeapon)
                {
                    Debug.Log("El arma activa es la misma que la que se recoge.");
                    return;
                }
                else if(slots.Contains(newWeapon))
                {
                    Debug.Log("El arma que se recoge ya está en el inventario.");
                    return;
                }
                if(weaponActive != null)
                {
                    int slotIndex = Array.FindIndex(slots, weapon => weapon == weaponActive); // Obtiene el índice del arma actual
                    Debug.Log(slotIndex);
                    slots[slotIndex] = newWeapon; // Cambia el arma actual por la nueva
                    ChangeWeapon((uint)slotIndex); // Cambia el arma en el inventario
                }
                else
                {
                    Debug.Log("No se encontró el arma activa en el inventario.");
                }
               
            }
           
        }
    } 
    
    public bool HasGorrocoptero()
    {
        hasGorrocoptero = false;

        foreach (var weapon in slots)
        {
            if (weapon != null && weapon.weaponName == "FPGorrocoptero")
            {
                hasGorrocoptero = true;
                break;
            }
        }
        return hasGorrocoptero;
    }

    
}