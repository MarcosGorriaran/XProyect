using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Gorrocoptero : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponSO weaponSO;

    public void Attack()
    {
        Debug.Log("Gorrocoptero Attack");
    }

    public void DisactiveBullet(GameObject bullet)
    {
        Debug.Log("Gorrocoptero DisactiveBullet");
    }

    public int GetPoolCount()
    {
        Debug.Log("Gorrocoptero GetPoolCount");
        return 0;
    }

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    public void Recharge()
    {
        Debug.Log("Gorrocoptero Recharge");
    }

    public void SetPoolCount(int count)
    {
        Debug.Log("Gorrocoptero SetPoolCount");
    }

    public bool IsRecharged { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
