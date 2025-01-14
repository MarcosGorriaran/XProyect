using UnityEngine;

public class HeliPack : MonoBehaviour, IWeapon
{
    [SerializeField]
    private WeaponSO weaponSO;
    public Rigidbody playerPhysics;
    [SerializeField]
    private float floatStrength;
    private float previousDrag;
    public void Attack()
    {
        if(playerPhysics.drag!= floatStrength)
        {
            previousDrag = playerPhysics.drag;
            playerPhysics.drag = floatStrength;
        }
        
    }
    public void StopAttack()
    {
        playerPhysics.drag = previousDrag;
    }

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }
}
