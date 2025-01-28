using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorDirectionNode : MonoBehaviour
{
    public event Action<DamageIndicatorDirectionNode> onLifeTimeExpire;
    public Vector3 damageSourcePosition;
    public Transform victimTransform;
    public float lifeTime;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(KillIndicatorOn());
    }
    void Update()
    {
        Vector3 Direction = (damageSourcePosition-victimTransform.position).normalized;
        float angle = Vector3.SignedAngle(Direction, Vector3.forward, Vector3.up);
        transform.localRotation = Quaternion.Euler(new Vector3(0,0,angle));
    }
    private IEnumerator KillIndicatorOn()
    {
        yield return new WaitForSeconds(lifeTime);
        onLifeTimeExpire?.Invoke(this);
        gameObject.SetActive(false);
    }
}
