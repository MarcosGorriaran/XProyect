using System.Linq;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private WeaponSO[] slots;
    [SerializeField]  private GameObject[] firstPersonWeaponModels;
    private IWeapon activeWeapon; // Referencia al arma activa
    private Transform weaponHolder;

    private void Awake()
    {
        Transform rightHand = GetComponentsInChildren<Transform>()
                           .FirstOrDefault(t => t.name == "mixamorig:RightHand");
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

        // Destruir el arma actual
        foreach (Transform child in weaponHolder)
        {
            Destroy(child.gameObject);
        }

        // Instancia la nueva arma
        GameObject weaponInstance = Instantiate(slots[slotIndex].weaponPrefab, weaponHolder);
        weaponInstance.transform.localPosition = slots[slotIndex].positionOffset;
       
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
}