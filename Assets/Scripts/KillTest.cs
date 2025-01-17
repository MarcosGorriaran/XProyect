using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out HPManager hPManager))
        {
            hPManager.Hurt(hPManager.GetMaxHp());
        }
    }
}
