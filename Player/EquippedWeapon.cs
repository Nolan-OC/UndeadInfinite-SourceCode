using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedWeapon : MonoBehaviour
{
    public Animator anim;

    public WeaponStats weaponStats;
    public GameObject handContainer;    //contains space where all weapons are held, only unequipped
    [SerializeField]private List<GameObject> weapons;    //contains a list of each weapon for cycling through
    public GameObject equippedWeapon;   //current gameobject containing mesh / scripts for weapon
    [SerializeField] bool attackAvailable = true;

    [Header("Weapon Info")]
    public string weaponName;
    public bool isRanged;
    public bool isThrown;
    public int weaponCode;

    public float weaponSightRange;
    public float weaponDamage;
    public float weaponAttackDelay;
    public float weaponKnockback;

    public string animationAttackName;
    //Reloading
    /*
    private int shotsFired;
    private int magSize;
    private float reloadTime;
    */

    private void Awake()
    {
        foreach (Transform child in handContainer.transform)
        {
            weapons.Add(child.gameObject);
        }
    }
    public void Start()
    {
        SwitchWeapons(weaponStats);
    }

    public void SwitchWeapons(WeaponStats newWeaponStats)
    {
        foreach (GameObject weapon in weapons)
        {
            if(weapon.name == newWeaponStats.weaponName)
            {
                equippedWeapon.SetActive(false);
                equippedWeapon = weapon;
                equippedWeapon.SetActive(true);
                //change equipped weapon mesh on and old weapon mesh off
            }
        }

        weaponStats = newWeaponStats;

        weaponName = newWeaponStats.weaponName;
        isRanged = newWeaponStats.isRanged;
        isThrown = !newWeaponStats.hasMuzzleFlash;
        weaponCode = newWeaponStats.weaponCode;

        weaponSightRange = newWeaponStats.weaponSightRange;
        GetComponent<FollowerCombat>().UpdateAttackRange();
        weaponDamage = newWeaponStats.weaponDamage;
        weaponAttackDelay = newWeaponStats.weaponAttackDelay;
        weaponKnockback = newWeaponStats.weaponKnockback;

        animationAttackName = newWeaponStats.attackAnimationName;
    }
    public void AttackAction()
    {
        if (isRanged)
            StartCoroutine("RangedAttack");
        else if (!isRanged)
            StartCoroutine("MeleeAttack");
    }
    private IEnumerator MeleeAttack()
    {
        if (attackAvailable&&!isRanged)
        {
            Collider collider = equippedWeapon.GetComponent<Collider>();

            attackAvailable = false;
            float waitTime = DetermineAnimationTimers(animationAttackName);
            anim.Play(animationAttackName);

            yield return new WaitForSeconds(.225f);
            collider.enabled = true;
            yield return new WaitForSeconds(waitTime-.225f);
            collider.enabled = false; //remove attack collider when not attacking
            yield return new WaitForSeconds(weaponAttackDelay - .225f - waitTime);
            attackAvailable = true;
            yield return null;
        }
    }
    private float DetermineAnimationTimers(string animName)
    {
        if (animName == "Swing")
            return .44f;
        else if (animName == "Stab")
            return .37f;
        else if (animName == "Punch")
            return .33f;
        else
        {
            return 0;
        }
    }
    private IEnumerator RangedAttack()
    {
        if (attackAvailable && !isThrown)
            {
                attackAvailable = false;
                anim.Play(animationAttackName);
                if (!GetComponent<HumanSurvivor>().isPlayer)
                    equippedWeapon.GetComponent<RangedWeaponAttack>().Fire(transform, false);
                else
                    equippedWeapon.GetComponent<RangedWeaponAttack>().Fire(transform, true);
                yield return new WaitForSeconds(weaponAttackDelay);
                attackAvailable = true;
                yield return null;
            }
            else if (attackAvailable && isThrown)
            {
                attackAvailable = false;
                yield return new WaitForSeconds(.225f);
                anim.Play(animationAttackName);
                if (!GetComponent<HumanSurvivor>().isPlayer)
                    equippedWeapon.GetComponent<RangedWeaponAttack>().Fire(transform, false);
                else
                    equippedWeapon.GetComponent<RangedWeaponAttack>().Fire(transform, true);

                yield return new WaitForSeconds(weaponAttackDelay - .225f);
                attackAvailable = true;
                yield return null;
            }
    }
    public void ToggleHitBox()
    {
        Collider hitBox = null;

        if (TryGetComponent(out Collider collider))
            hitBox = equippedWeapon.GetComponent<Collider>();

        if (hitBox != null)
        {
            if (!hitBox.enabled)
                hitBox.enabled = false;
            else
                hitBox.enabled = true;
        }
    }
}
