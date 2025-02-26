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
    public event Action<GameObject> onDeathBy;
    public event Action<GameObject> onRevive;
    public event Action<float> onHPChange;
    public event Action<float,GameObject> onHPChangeBy;
    public AudioSource audioSource;
    public AudioClip hittedSound;
    // Start is called before the first frame update

    private void Awake()
    {
        hp = maxHp;
    }
    public void Hurt(float damage)
    {
        if (!IsDead())
        {
            audioSource.PlayOneShot(hittedSound);
            if (damage < 0) damage = 0;
            hp -= damage;
            onHPChange?.Invoke(-damage);
            if (IsDead())
            {
                onDeath?.Invoke();
            }

        }
    }
    public void Hurt(float damage, GameObject source)
    {
        if (!IsDead())
        {
            if (damage < 0) damage = 0;
            hp -= damage;
            audioSource.PlayOneShot(hittedSound);
            onHPChangeBy?.Invoke(-damage,source);
            onHPChange?.Invoke(damage);
            if (IsDead())
            {
                onDeathBy?.Invoke(source);
                onDeath?.Invoke();
            }

        }
    }
    public void Heal(float healAmount)
    {
        if (!IsDead())
        {
            hp+= healAmount;
            if (hp > maxHp) hp = maxHp;
            onHPChange?.Invoke(healAmount);
        }
    }
    public void Revive()
    {
        hp = maxHp;
        onHPChange?.Invoke(maxHp);
        onRevive?.Invoke(gameObject);
    }
    public bool IsDead() { return hp <= 0; }
    public float GetHp() { return hp; }
    public float GetMaxHp() { return maxHp; }
}
