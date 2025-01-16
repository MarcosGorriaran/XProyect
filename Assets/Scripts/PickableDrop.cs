using System.Collections;
using UnityEngine;

public class PickableDrop : MonoBehaviour
{
    public WeaponSO weapon;
    private GameObject objectModel;
    [SerializeField]
    private float waitTime;
    private void Awake()
    {
        objectModel = Instantiate(weapon.weaponModel,transform);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Inventory inventory) && objectModel.activeSelf)
        {
            if (inventory.AddWeapon(weapon))
            {
                objectModel.SetActive(false);
                StartCoroutine(WaitSpawn());
            }
        }
    }
    private IEnumerator WaitSpawn()
    {
        yield return new WaitForSeconds(waitTime);
        objectModel.SetActive(true);
    }
}
