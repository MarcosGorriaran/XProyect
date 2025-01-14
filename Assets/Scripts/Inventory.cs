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
    public void AddWeapon(WeaponSO weapon)
    {
        int nullIndex = slots.Select((value,index)=>new { value, index })
                        .Where(x => x.value == null)
                        .Select(x => x.index).FirstOrDefault();
        slots[nullIndex] = weapon;
    }
}
