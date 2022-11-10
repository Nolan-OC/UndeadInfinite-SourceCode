using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponAttack : MonoBehaviour
{
    public WeaponStats weapon;
    public List<GameObject> FirePoints;
    public GameObject projectile;
    public GameObject muzzleFlash;
    [SerializeField]private bool hasMuzzleFlash;
    //--AUDIO--
    public AudioSource audioSource;
    public AudioClip gunshot;
    //Aiming
    private GameObject aimTarget;
    private Vector3 bulletTrajectory;
    //private int[] firepointOffsets= new int[5] {0,5,-5,10,-10};
    [SerializeField] private List<int> firepointOffsets;

    private void Awake()
    {
        projectile.GetComponent<Bullet>().damage = weapon.weaponDamage;
        projectile.GetComponent<Bullet>().knockback = weapon.weaponKnockback;
        projectile.GetComponent<Bullet>().speed = weapon.weaponProjectileSpeed;
        projectile.GetComponent<Bullet>().penetration = weapon.projectilePenetration;
        audioSource = GetComponent<AudioSource>();

        foreach (Transform child in transform)
        {
             FirePoints.Add(child.gameObject);
        }
    }

    public void Fire(Transform character,bool isPlayer)
    {
            Vector3 quat = Quaternion.AngleAxis(20, character.up) * character.forward;
            Vector3 oquat = Quaternion.AngleAxis(-20, character.up) * character.forward;
            Debug.DrawRay(character.position, character.forward, Color.magenta, 1);
            Debug.DrawRay(character.position, quat, Color.black, 1);
            Debug.DrawRay(character.position, oquat, Color.black, 1);

            audioSource.PlayOneShot(gunshot);
            int i = 0;
            foreach (GameObject firepoint in FirePoints)
            {
                float degOffset = firepointOffsets[i];

                Vector3 forwardDir = new Vector3(0, 0, 0);
                if (!isPlayer)
                    forwardDir = Quaternion.AngleAxis(degOffset, character.up) * bulletTrajectory;
                else
                    forwardDir = Quaternion.AngleAxis(degOffset, character.up) * character.forward;

                Bullet bullet = Instantiate(projectile, firepoint.transform.position, Quaternion.LookRotation(forwardDir)).GetComponent<Bullet>();
                bullet.penetration = weapon.projectilePenetration;
                bullet.damage = weapon.weaponDamage;
                bullet.speed = weapon.weaponProjectileSpeed;
                bullet.knockback = weapon.weaponKnockback;

                i++;
            }
            if (hasMuzzleFlash)
                StartCoroutine("MuzzleFlash");
    }
    public void Aim(GameObject enemyToAttack)
    {
        aimTarget = enemyToAttack;
        StartCoroutine("TrackTarget");
    }
    private IEnumerator TrackTarget()
    {
        float time = 0;
        while(time<.5)
        {
            Vector3 lookPos = new Vector3(aimTarget.transform.position.x - transform.position.x, 0, aimTarget.transform.position.z - transform.position.z);
            Debug.DrawRay(transform.position, lookPos, Color.yellow,5f);
            time += Time.deltaTime;

            transform.localEulerAngles = lookPos;
            bulletTrajectory = lookPos;
            time += Time.deltaTime;
        }
        yield return null;
    }
    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(.05f);
        muzzleFlash.SetActive(false);
        yield return null;
    }
}
