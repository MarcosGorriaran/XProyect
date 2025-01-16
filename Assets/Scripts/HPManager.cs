using System;
using System.ComponentModel;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    [SerializeField]
    float hp;
    [SerializeField]
    float maxHp;
    public event Action onDeath;
    public event Action onRevive;
    public event Action<float> onHPChange;
    // Start is called before the first frame update

    private void Awake()
    {
        hp = maxHp;
    }
    public void Hurt(float damage)
    {
        if (!IsDead())
        {
            hp -= damage;
            onHPChange?.Invoke(-damage);
            if (IsDead())
            {
                onDeath?.Invoke();
            }

        }
    }
    public void Revive()
    {
        hp = maxHp;
        onHPChange?.Invoke(maxHp);
        onRevive?.Invoke();
    }
    public bool IsDead() { return hp <= 0; }
    public float GetHp() { return hp; }
    public float GetMaxHp() { return maxHp; }
}
