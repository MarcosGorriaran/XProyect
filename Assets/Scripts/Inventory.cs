using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private WeaponSO[] slots;
    
    public WeaponSO ChangeWeapon(uint slotIndex)
    {
        return slots[slotIndex];
    }
    public bool AddWeapon(WeaponSO weapon)
    {
        if (slots.Where(value=>value==null).Count()<=0)
        {
            return false;
        }
        int nullIndex = slots.Select((value,index)=>new { value, index })
                        .Where(x => x.value == null)
                        .Select(x => x.index).FirstOrDefault();
        slots[nullIndex] = weapon;
        return true;
    }
    public bool RemoveWeapon(WeaponSO weapon)
    {
        if (slots.Where(value => value == weapon).Count() <= 0)
        {
            return false;
        }
        int weaponIndex = slots.Select((value, index) => new { value, index })
                        .Where(x => x.value == weapon)
                        .Select(x => x.index).FirstOrDefault();
        if (slots[weaponIndex] == null)
        {
            return false;
        }
        slots[weaponIndex] = null;
        return true;
    }
    public bool RemoveWeapon(uint slotIndex)
    {
        if (slots[slotIndex] == null)
        {
            return false;
        }
        slots[slotIndex] = null;
        return true;
    }
}
