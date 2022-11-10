using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Stats", menuName = "Scriptable Object/WeaponStats", order = 1)]
public class WeaponStats : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public bool isRanged;
    public int weaponCode;  //for use which bullet to fire

    [Header("Combat Stats")]
    public float weaponSightRange;
    public float weaponDamage;
    public float weaponAttackDelay;
    public float weaponKnockback;

    [Header("Projectile Stats")]
    public float weaponProjectileSpeed;
    public int projectilePenetration;
    public bool hasMuzzleFlash;

    [Header("Animation Logic")]
    public string attackAnimationName;
}
