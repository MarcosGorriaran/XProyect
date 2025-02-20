using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public Vector3 positionOffset; //no implementa ahora mismo creo
}