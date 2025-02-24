using UnityEngine;

public class Shield : MonoBehaviour, IWeapon
{
    private string targetTag = "Bullet";
    public WeaponSO weaponSO;

    public bool IsRecharged { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void DisactiveBullet(GameObject bullet)
    {
        throw new System.NotImplementedException();
    }

    public int GetPoolCount()
    {
        throw new System.NotImplementedException();
    }

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    public void Recharge()
    {
        throw new System.NotImplementedException();
    }

    public void SetPoolCount(int count)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Destroy(other.gameObject);
        }
    }
}
