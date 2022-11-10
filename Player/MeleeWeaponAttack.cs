using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponAttack : MonoBehaviour
{
    public WeaponStats weapon;
    private float damage;
    private float knockback;

    private void Awake()
    {
        damage = weapon.weaponDamage;
        knockback = weapon.weaponKnockback;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Zombie zombieScript))
        {
            zombieScript.TakeMeleeDamage(damage, knockback);
        }
    }
}
